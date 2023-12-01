using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Hotels;
using Entities.Payments;
using Entities.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationDal _reservationDal;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        public ReservationService(IReservationDal reservationDal, IMapper mapper, IRoomService roomService, IPaymentService paymentService, IUserService userService)
        {
            _reservationDal = reservationDal;
            _mapper = mapper;
            _roomService = roomService;
            _paymentService = paymentService;
            _userService = userService;
        }

        public async Task<Result<Reservation>> Reserve(CreateReservationDto reservation)
        {
            var roomResult = await _roomService.GetByIdAsync(reservation.RoomId);
            if (!roomResult.Success)
                return Result<Reservation>.FailureResult(roomResult.Message);

            var userToCheck = await _userService.GetByIdAsync(reservation.CustomerId);
            if (!userToCheck.Success)
                return Result<Reservation>.FailureResult(userToCheck.Message);

            var result = CheckIfTheCustomerHasAnyReservationsBetweenTheDates
               (reservation.CustomerId, (DateTime)reservation.CheckInDate, (DateTime)reservation.CheckOutDate);
            if (!result.Success)
                return Result<Reservation>.FailureResult(result.Message);

            result = CheckIfTheRoomIsOccupiedOnTheReservationDate(reservation);
            if (!result.Success)
                return Result<Reservation>.FailureResult(result.Message);

            var paymentResult = await Pay(reservation, (decimal)roomResult.Data!.Price);
            if (!paymentResult.Success)
                return Result<Reservation>.FailureResult(paymentResult.Message);

            var newReservation = _mapper.Map<Reservation>(reservation);
            newReservation.PaymentId = paymentResult.Data!.Id;
            newReservation = await _reservationDal.AddAsync(newReservation);

            var saved = await _reservationDal.SaveAsync();
            if (saved == 0)
            {
                await _paymentService.CancelPayment(paymentResult.Data!.Id);
                return Result<Reservation>.FailureResult("Rezervasyon oluşturulamadı");
            }
            return Result<Reservation>.SuccessResult(newReservation, "Rezervasyon oluşturuldu");

        }

        public Result<List<Reservation>> GetAll()
        {
            var reservations = _reservationDal.GetAll().ToList();
            return reservations.Count == 0
                ? Result<List<Reservation>>.FailureResult("Rezervasyonlar bulunamadı")
                : Result<List<Reservation>>.SuccessResult(reservations, "Rezervasyonlar bulundu");
        }

        public Result<PaginationResult<Reservation>> GetAllPagination(BasePaginationRequest req)
        {
            var reservations = _reservationDal.GetWithPagination(req);
            return reservations == null || reservations.TotalCount == 0
                ? Result<PaginationResult<Reservation>>.FailureResult("Rezervasyonlar bulunamadı")
                : Result<PaginationResult<Reservation>>.SuccessResult(reservations, "Rezervasyonlar bulundu");
        }

        public async Task<Result<Reservation>> GetByIdAsync(int id)
        {
            var reservation = await _reservationDal.GetByIdAsync(id);
            return reservation == null
                ? Result<Reservation>.FailureResult("Rezervasyon bulunamadı")
                : Result<Reservation>.SuccessResult(reservation, "Rezervasyon bulundu");
        }

        public async Task<Result<Reservation>> Remove(RemoveReservationDto reservation)
        {
            var reservationToBeRemoved = _mapper.Map<Reservation>(reservation);
            reservationToBeRemoved = await _reservationDal.SoftRemoveAsync(reservationToBeRemoved.Id);

            var saved = await _reservationDal.SaveAsync();
            return saved == 0
                ? Result<Reservation>.FailureResult("Rezervasyon silinemedi")
                : Result<Reservation>.SuccessResult(reservationToBeRemoved, "Rezervasyon silindi");
        }

        public async Task<Result<Reservation>> RemoveById(int id)
        {
            var reservationToBeRemoved = await _reservationDal.SoftRemoveAsync(id);

            var saved = await _reservationDal.SaveAsync();
            return saved == 0
                ? Result<Reservation>.FailureResult("Rezervasyon silinemedi")
                : Result<Reservation>.SuccessResult(reservationToBeRemoved, "Rezervasyon silindi");
        }

        public async Task<Result<List<Reservation>>> RemoveRange(List<RemoveReservationDto> reservation)
        {
            var reservationsToBeRemoved = _mapper.Map<List<Reservation>>(reservation);
            reservationsToBeRemoved = _reservationDal.RemoveRange(reservationsToBeRemoved);

            var saved = await _reservationDal.SaveAsync();
            return saved == 0
                ? Result<List<Reservation>>.FailureResult("Rezervasyonlar silinemedi")
                : Result<List<Reservation>>.SuccessResult(reservationsToBeRemoved, "Rezervasyonlar silindi");
        }

        public async Task<Result<Reservation>> Update(UpdateReservationDto reservation)
        {
            var reservationToBeUpdated = _mapper.Map<Reservation>(reservation);
            reservationToBeUpdated = _reservationDal.Update(reservationToBeUpdated);

            var saved = await _reservationDal.SaveAsync();
            return saved == 0
                ? Result<Reservation>.FailureResult("Rezervasyon güncellenemedi")
                : Result<Reservation>.SuccessResult(reservationToBeUpdated, "Rezervasyon güncellendi");
        }
        public async Task<Result<Reservation>> CancelReservationById(int id)
        {
            var reservation = await _reservationDal.GetByIdAsync(id);
            if (reservation == null)
                return Result<Reservation>.FailureResult("Rezervasyon bulunamadı");
            reservation = await _reservationDal.SoftRemoveAsync(id);
            reservation.Status = false;
            var result = await _reservationDal.SaveAsync();
            if (result == 0)
                return Result<Reservation>.FailureResult("Rezervasyon iptal edilemedi");

            var cancelPaymentResult = await _paymentService.CancelPayment((int)reservation.PaymentId);
            if (!cancelPaymentResult.Success)
            {
                reservation.IsDeleted = false;
                await _reservationDal.SaveAsync();
                return Result<Reservation>.FailureResult(cancelPaymentResult.Message);
            }

            return Result<Reservation>.SuccessResult(reservation, "Rezervasyon iptal edildi");
        }
        private Result<string> CheckIfTheRoomIsOccupiedOnTheReservationDate(CreateReservationDto dto)
        {
            if (dto.CheckInDate == null || dto.CheckOutDate == null || dto.CheckInDate >= dto.CheckOutDate || dto.CheckInDate < DateTime.UtcNow || dto.CheckOutDate < DateTime.UtcNow)
            {
                return Result<string>.FailureResult("Geçerli bir giriş ve çıkış tarihi seçilmelidir.");
            }

            var reservations = _reservationDal
                .GetAll().Where(x => x.RoomId == dto.RoomId
                && (x.CheckInDate <= dto.CheckOutDate
                    && x.CheckOutDate >= dto.CheckInDate)).ToList();

            if (reservations.Count > 0)
            {
                return Result<string>.FailureResult("Seçilen tarihlerde oda doludur.");
            }

            return Result<string>.SuccessResult(string.Empty);
        }

        private Result<string> CheckIfTheCustomerHasAnyReservationsBetweenTheDates(int customerId, DateTime checkInDate, DateTime checkOutDate)
        {
            if (checkInDate == null || checkOutDate == null || checkInDate >= checkOutDate || checkInDate < DateTime.UtcNow || checkOutDate < DateTime.UtcNow)
            {
                return Result<string>.FailureResult("Geçerli bir giriş ve çıkış tarihi seçilmelidir.");
            }
            var reservations = _reservationDal
                .GetAll().Where(x => x.CustomerId == customerId
                && (x.CheckInDate <= checkOutDate
                    && x.CheckOutDate >= checkInDate)).ToList();

            return reservations.Count > 0
                ? Result<string>.FailureResult("Seçilen tarihlerde kullanıcının başka bir rezervasyonu bulunmaktadır")
                : Result<string>.SuccessResult(string.Empty);

        }


        private async Task<Result<Payment>> Pay(CreateReservationDto reservation, decimal roomPrice)
        {
            var paymentDto = new CreatePaymentDto
            {
                Amount = roomPrice,
                CardNumber = reservation.paymentDto.CardNumber,
                CardHolderName = reservation.paymentDto.CardHolderName,
                ExpirationDate = reservation.paymentDto.ExpirationDate,
                CVV = reservation.paymentDto.CVV,
                PaymentDate = DateTime.UtcNow

            };
            var payment = _mapper.Map<Payment>(paymentDto);

            return await _paymentService.PayAsync(paymentDto, (DateTime)reservation.CheckOutDate, (DateTime)reservation.CheckInDate);
        }

      
    }
}

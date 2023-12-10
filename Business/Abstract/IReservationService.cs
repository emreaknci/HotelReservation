using Core.Entities;
using Core.Utils.Results;
using Entities.Hotels;
using Entities.Reservation;
using Entities.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Result<List<Reservation>> GetAll();
        Task<Result<Reservation>> GetByIdAsync(int id);
        Result<List<ReservationListDto>> GetAllByCustomerId(int customerId);
        Result<List<ReservationListDto>> GetAllWithDetails();
        Result<List<ReservationListDto>> GetAllPastReservationsByCustomerId(int customerId);
        Result<List<ReservationListDto>> GetAllActiveReservationsByCustomerId(int customerId);
        Result<List<ReservationListDto>> GetAllCanceledReservationsByCustomerId(int customerId);
        Task<Result<Reservation>> Reserve(CreateReservationDto reservation);
        Task<Result<Reservation>> CancelReservationById(int id);
        Result<string> CheckCustomerBookingAndRoomOccupancy(ReservationCheckDto dto);
        Task<Result<Reservation>> Update(UpdateReservationDto reservation);
        Task<Result<Reservation>> Remove(RemoveReservationDto reservation);
        Task<Result<Reservation>> RemoveById(int id);
        Task<Result<List<Reservation>>> RemoveRange(List<RemoveReservationDto> reservation);
        Result<PaginationResult<Reservation>> GetAllPagination(BasePaginationRequest req);

    }
}

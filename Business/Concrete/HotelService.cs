using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using DataAccess.Abstract;
using Entities.Hotels;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class HotelService : IHotelService
    {
        private readonly IHotelDal _hotelDal;
        private readonly IMapper _mapper;

        public HotelService(IHotelDal hotelDal, IMapper mapper)
        {
            _hotelDal = hotelDal;
            _mapper = mapper;
        }

        public async Task<Result<Hotel>> AddAsync(CreateHotelDto hotel)
        {
            var newHotel = _mapper.Map<Hotel>(hotel);
            await _hotelDal.AddAsync(newHotel);
            await _hotelDal.SaveAsync();
            return Result<Hotel>.SuccessResult(newHotel, "Otel eklendi.");
        }

        public async Task<Result<List<Hotel>>> AddRangeAsync(List<CreateHotelDto> hotels)
        {
            var newHotels = _mapper.Map<List<Hotel>>(hotels);
            await _hotelDal.AddRangeAsync(newHotels);
            await _hotelDal.SaveAsync();

            return Result<List<Hotel>>.SuccessResult(newHotels, "Oteller eklendi.");
            throw new NotImplementedException();
        }

        public Result<List<Hotel>> GetAll()
        {
            var hotels = _hotelDal.GetAll().ToList();
            return Result<List<Hotel>>.SuccessResult(hotels, "Oteller listelendi.");
        }

        public Result<PaginationResult<Hotel>> GetAllPagination(BasePaginationRequest req)
        {
            var hotels = _hotelDal.GetWithPagination(req);
            return Result<PaginationResult<Hotel>>.SuccessResult(hotels, "Oteller listelendi.");
        }

        public async Task<Result<Hotel>> GetByIdAsync(int id)
        {
            var hotel = await _hotelDal.GetByIdAsync(id);
            return Result<Hotel>.SuccessResult(hotel, "Otel getirildi.");
        }

        public async Task<Result<Hotel>> Remove(RemoveHotelDto hotel)
        {
            var hotelToBeDeleted = _mapper.Map<Hotel>(hotel);
            hotelToBeDeleted = await _hotelDal.SoftRemoveAsync(hotelToBeDeleted.Id);
            await _hotelDal.SaveAsync();
            return Result<Hotel>.SuccessResult(hotelToBeDeleted, "Otel silindi.");
        }

        public async Task<Result<Hotel>> RemoveById(int id)
        {
            var hotelToBeDeleted = await _hotelDal.SoftRemoveAsync(id);
            await _hotelDal.SaveAsync();
            return Result<Hotel>.SuccessResult(hotelToBeDeleted, "Otel silindi.");

        }

        public async Task<Result<List<Hotel>>> RemoveRange(List<RemoveHotelDto> hotels)
        {
            var hotelsToBeDeleted = _mapper.Map<List<Hotel>>(hotels);
            hotelsToBeDeleted.ForEach(async hotel =>
            {
                await _hotelDal.SoftRemoveAsync(hotel.Id);
            });
            await _hotelDal.SaveAsync();

            return Result<List<Hotel>>.SuccessResult(hotelsToBeDeleted, "Oteller silindi.");
        }

        public async Task<Result<Hotel>> Update(UpdateHotelDto hotel)
        {
            var hotelToBeUpdated = _mapper.Map<Hotel>(hotel);
            _hotelDal.Update(hotelToBeUpdated);
            await _hotelDal.SaveAsync();
            return Result<Hotel>.SuccessResult(hotelToBeUpdated, "Otel bilgileri güncellendi.");
        }

        public async Task<Result<List<Hotel>>> UpdateRange(List<UpdateHotelDto> hotels)
        {
            var hotelsToBeUpdated = _mapper.Map<List<Hotel>>(hotels);
            _hotelDal.UpdateRange(hotelsToBeUpdated);
            await _hotelDal.SaveAsync();
            return Result<List<Hotel>>.SuccessResult(hotelsToBeUpdated, "Otellerin bilgileri güncellendi.");

        }


    }
}

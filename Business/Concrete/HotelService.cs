using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Helpers;
using Core.Utils.Results;
using DataAccess.Abstract;
using Entities.HotelImages;
using Entities.Hotels;
using Entities.Rooms;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class HotelService : IHotelService
    {
        private readonly IHotelDal _hotelDal;
        private readonly IMapper _mapper;
        private readonly IHotelImageDal _hotelImageDal;
        private readonly IRoomService _roomService;

        public HotelService(IHotelDal hotelDal, IMapper mapper, IHotelImageDal hotelImageDal, IRoomService roomService)
        {
            _hotelDal = hotelDal;
            _mapper = mapper;
            _hotelImageDal = hotelImageDal;
            _roomService = roomService;
        }

        public async Task<Result<Hotel>> AddAsync(CreateHotelDto hotel)
        {

            var newHotel = _mapper.Map<Hotel>(hotel);
            newHotel = await _hotelDal.AddAsync(newHotel);
            var saved = await _hotelDal.SaveAsync();

            if (saved < 0)
                return Result<Hotel>.FailureResult("Otel eklenemedi.");

            if (hotel.Images != null)
            {
                foreach (var image in hotel.Images)
                {
                    var result = FileHelper.Upload(image);
                    if (!result.Success)
                        return Result<Hotel>.FailureResult(result.Message);
                    var newImage = new HotelImage
                    {
                        HotelId = newHotel.Id,
                        ImageUrl = result.Data
                    };
                    await _hotelImageDal.AddAsync(newImage);
                    saved = await _hotelImageDal.SaveAsync();
                    if (saved < 0)
                        return Result<Hotel>.FailureResult("Otel resimleri eklenemedi.");
                }
            }
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

        public Result<List<HotelDetailDto>> GetAllWithImages()
        {
            var hotels = _hotelDal.GetHotelsWithImages();

            return hotels == null || hotels.Count == 0
                ? Result<List<HotelDetailDto>>.FailureResult("Oteller bulunamadı.")
                : Result<List<HotelDetailDto>>.SuccessResult(hotels, "Oteller listelendi.");

        }

        public async Task<Result<Hotel>> GetByIdAsync(int id)
        {
            var hotel = await _hotelDal.GetByIdAsync(id);
            return Result<Hotel>.SuccessResult(hotel, "Otel getirildi.");
        }

        public Result<HotelDetailDto> GetByIdWithImages(int id)
        {
            var hotel = _hotelDal.GetHotelWithImagesById(id);

            if (hotel == null)
            {
                return Result<HotelDetailDto>.FailureResult("Otel bulunamadı.");
            }

            var rooms = _roomService.GetRoomsWithImagesByHotelId(id);
            hotel.Rooms = rooms?.Data != null ? rooms.Data : null;
            return Result<HotelDetailDto>.SuccessResult(hotel, "Otel getirildi.");
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

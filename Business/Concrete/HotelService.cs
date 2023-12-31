﻿using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Helpers;
using Core.Utils.Results;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.HotelImages;
using Entities.Hotels;
using Entities.Rooms;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<bool>> ChangeHotelStatus(int hotelId)
        {
            var hotel = await _hotelDal.GetByIdAsync(hotelId);
            if (hotel == null)
                return Result<bool>.FailureResult("Otel bulunamadı.");

            hotel.Status = !hotel.Status;
            _hotelDal.Update(hotel);
            var saved = await _hotelDal.SaveAsync();

            return saved > 0
                ? Result<bool>.SuccessResult(hotel.Status, "Otel durumu güncellendi.")
                : Result<bool>.FailureResult("Otel durumu güncellenemedi.");
        }

        public Result<List<Hotel>> GetAll()
        {
            var hotels = _hotelDal.GetAll().ToList();
            return Result<List<Hotel>>.SuccessResult(hotels, "Oteller listelendi.");
        }

        public Result<List<HotelDto>> GetAllForDropdown()
        {

            var result = _hotelDal.GetAll()
             .Select(x => new HotelDto
             {
                 Id = x.Id,
                 Name = x.Name,
                 Status = x.Status
             })
           .ToList();

            return result == null || result.Count == 0
                ? Result<List<HotelDto>>.FailureResult("Oteller bulunamadı.")
                : Result<List<HotelDto>>.SuccessResult(result, "Oteller listelendi.");

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

        public Result<HotelDetailDto> GetByIdWithImagesAndRooms(int id)
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
        public Result<HotelDetailDto> GetByIdWithImages(int id)
        {
            var hotel = _hotelDal.GetHotelWithImagesById(id);

            return hotel == null
             ? Result<HotelDetailDto>.FailureResult("Otel bulunamadı.")
             : Result<HotelDetailDto>.SuccessResult(hotel, "Otel getirildi.");
        }

        public async Task<Result<Hotel>> SoftRemoveAsync(RemoveHotelDto hotel)
        {
            var hotelToBeDeleted = _mapper.Map<Hotel>(hotel);
            hotelToBeDeleted = await _hotelDal.SoftRemoveAsync(hotelToBeDeleted.Id);
            await _hotelDal.SaveAsync();
            return Result<Hotel>.SuccessResult(hotelToBeDeleted, "Otel silindi.");
        }

        public async Task<Result<Hotel>> SoftRemoveAsyncById(int id)
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

        public async Task<Result<Hotel>> UpdateAsync(UpdateHotelDto hotel)
        {
           
            if (hotel.ImagePathsToDelete != null)
            {
                var result = _hotelImageDal.RemoveRange(hotel.ImagePathsToDelete);
                if (!result)
                    return Result<Hotel>.FailureResult("Otel resimleri silinirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }
            var saved = 0;
            if (hotel.NewImages != null)
            {
                foreach (var image in hotel.NewImages)
                {
                    var result = FileHelper.Upload(image);
                    if (!result.Success)
                        return Result<Hotel>.FailureResult(result.Message);
                    var newImage = new HotelImage
                    {
                        HotelId = hotel.Id,
                        ImageUrl = result.Data
                    };
                    newImage=await _hotelImageDal.AddAsync(newImage);
                    saved = await _hotelImageDal.SaveAsync();
                    if (saved < 0)
                        return Result<Hotel>.FailureResult("Otel resimleri eklenemedi.");
                }
            }

            var hotelToBeUpdated = _mapper.Map<Hotel>(hotel);
            _hotelDal.Update(hotelToBeUpdated);
            saved=await _hotelDal.SaveAsync();

            return saved == 0
                ? Result<Hotel>.FailureResult("Otel güncellenemedi.")
                : Result<Hotel>.SuccessResult(hotelToBeUpdated, "Otel güncellendi.");
        }

        public async Task<Result<List<Hotel>>> UpdateRange(List<UpdateHotelDto> hotels)
        {
            var hotelsToBeUpdated = _mapper.Map<List<Hotel>>(hotels);
            _hotelDal.UpdateRange(hotelsToBeUpdated);
            await _hotelDal.SaveAsync();
            return Result<List<Hotel>>.SuccessResult(hotelsToBeUpdated, "Otellerin bilgileri güncellendi.");

        }

        public async Task<Result<Hotel>> RemoveAsyncById(int id)
        {
            var hotel = await _hotelDal.GetByIdAsync(id);

            if (hotel == null)
                return Result<Hotel>.FailureResult("Otel bulunamadı");

            var result = _hotelImageDal.RemoveAllByHotelId(id);
            var saved = 0;
            if (result)
            {
                saved = await _hotelImageDal.SaveAsync();
                if (saved == 0)
                    return Result<Hotel>.FailureResult("Otel resimleri silinirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }

            var hotelRoomsResult = _roomService.GetRoomsByHotelId(id);
            if (hotelRoomsResult.Success)
            {
                var roomsRemoveResult = await _roomService.RemoveRangeAsyncByHotelId(id);
                if (!roomsRemoveResult.Success)
                    return Result<Hotel>.FailureResult(roomsRemoveResult.Message);
            }


            hotel = _hotelDal.Remove(hotel);
            saved = await _hotelDal.SaveAsync();
            return saved == 0
                ? Result<Hotel>.FailureResult("Otel silinemedi")
                : Result<Hotel>.SuccessResult(hotel, "Otel silindi");
        }

        public Result<List<HotelWithImageDto>> GetHotelsWithFirstImage(int? hotelCount = null)
        {
            var hotels = _hotelDal.GetHotelsWithImage(hotelCount);
            return hotels == null || hotels.Count == 0
                ? Result<List<HotelWithImageDto>>.FailureResult("Oteller bulunamadı.")
                : Result<List<HotelWithImageDto>>.SuccessResult(hotels, "Oteller listelendi.");
        }
    }
}

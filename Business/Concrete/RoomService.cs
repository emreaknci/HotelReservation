using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Helpers;
using Core.Utils.Results;
using DataAccess.Abstract;
using Entities.RoomImages;
using Entities.Rooms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RoomService : IRoomService
    {
        private readonly IRoomDal _roomDal;
        private readonly IMapper _mapper;
        private readonly IRoomImageDal _roomImageDal;

        public RoomService(IRoomDal roomDal, IMapper mapper, IRoomImageDal roomImageDal)
        {
            _roomDal = roomDal;
            _mapper = mapper;
            _roomImageDal = roomImageDal;
        }

        public async Task<Result<Room>> AddAsync(CreateRoomDto room)
        {
            var newRoom = _mapper.Map<Room>(room);
            newRoom = await _roomDal.AddAsync(newRoom);
            var saved = await _roomDal.SaveAsync();

            if (saved < 0)
                return Result<Room>.FailureResult("Oda eklenemedi.");

            if (room.Images != null)
            {
                foreach (var image in room.Images)
                {
                    var result = FileHelper.Upload(image);
                    if (!result.Success)
                        return Result<Room>.FailureResult(result.Message);
                    var newImage = new RoomImage
                    {
                        RoomId = newRoom.Id,
                        ImageUrl = result.Data
                    };
                    await _roomImageDal.AddAsync(newImage);
                    saved = await _roomImageDal.SaveAsync();
                    if (saved < 0)
                        return Result<Room>.FailureResult("Oda resimleri eklenemedi.");
                }
            }
            return Result<Room>.SuccessResult(newRoom, "Oda eklendi.");
        }

        public async Task<Result<List<Room>>> AddRangeAsync(List<CreateRoomDto> rooms)
        {
            var newRooms = _mapper.Map<List<Room>>(rooms);
            newRooms = await _roomDal.AddRangeAsync(newRooms);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<List<Room>>.FailureResult("Odalar eklenemedi")
                : Result<List<Room>>.SuccessResult(newRooms, "Odalar eklendi");

        }

        public async Task<Result<bool>> ChangeRoomStatus(int roomId)
        {
            var room = await _roomDal.GetByIdAsync(roomId);
            if (room == null)
                return Result<bool>.FailureResult("Oda bulunamadı");

            room.Status = !room.Status;
            room = _roomDal.Update(room);
            var saved = await _roomDal.SaveAsync();

            return saved == 0
                ? Result<bool>.FailureResult("Oda durumu değiştirilemedi")
                : Result<bool>.SuccessResult(room.Status, "Oda durumu değiştirildi");
        }

        public Result<List<Room>> GetAll()
        {
            var rooms = _roomDal.GetAll().ToList();
            return rooms == null || rooms.Count == 0
                ? Result<List<Room>>.FailureResult("Odalar bulunamadı")
                : Result<List<Room>>.SuccessResult(rooms, "Odalar bulundu");
        }

        public Result<PaginationResult<Room>> GetAllPagination(BasePaginationRequest req)
        {
            var rooms = _roomDal.GetWithPagination(req);
            return rooms == null || rooms.TotalCount == 0
                ? Result<PaginationResult<Room>>.FailureResult("Odalar bulunamadı")
                : Result<PaginationResult<Room>>.SuccessResult(rooms, "Odalar bulundu");
        }

        public async Task<Result<Room>> GetByIdAsync(int id)
        {
            var room = await _roomDal.GetByIdAsync(id);
            return room == null
                ? Result<Room>.FailureResult("Oda bulunamadı")
                : Result<Room>.SuccessResult(room, "Oda bulundu");
        }

        public Result<RoomDetailDto> GetByIdWithImages(int roomId)
        {
            var room = _roomDal.GetRoomWithImagesById(roomId);
            return room == null
                ? Result<RoomDetailDto>.FailureResult("Oda bulunamadı")
                : Result<RoomDetailDto>.SuccessResult(room, "Oda bulundu");
        }

        public Result<List<Room>> GetRoomsByHotelId(int hotelId)
        {
            var rooms = _roomDal.GetAll().Where(x => x.HotelId == hotelId).ToList();
            return rooms == null || rooms.Count == 0
                ? Result<List<Room>>.FailureResult("Otele ait oda bulunamadı")
                : Result<List<Room>>.SuccessResult(rooms, "Odalar bulundu");
        }

        public Result<List<RoomDetailDto>> GetRoomsWithImages()
        {
            var rooms = _roomDal.GetRoomsWithImages();
            return rooms == null || rooms.Count == 0
                ? Result<List<RoomDetailDto>>.FailureResult("Odalar bulunamadı")
                : Result<List<RoomDetailDto>>.SuccessResult(rooms, "Odalar bulundu");
        }

        public Result<List<RoomDetailDto>> GetRoomsWithImagesByHotelId(int hotelId)
        {
            var rooms = _roomDal.GetRoomsWithImagesByHotelId(hotelId);
            return rooms == null || rooms.Count == 0
                ? Result<List<RoomDetailDto>>.FailureResult("Otele ait oda bulunamadı")
                : Result<List<RoomDetailDto>>.SuccessResult(rooms, "Odalar bulundu");
        }

        public async Task<Result<Room>> SoftRemoveAsync(RemoveRoomDto room)
        {
            var roomToBeRemoved = _mapper.Map<Room>(room);
            roomToBeRemoved = await _roomDal.SoftRemoveAsync(roomToBeRemoved.Id);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda silinemedi")
                : Result<Room>.SuccessResult(roomToBeRemoved, "Oda silindi");
        }

        public async Task<Result<Room>> SoftRemoveAsyncById(int id)
        {
            var roomToBeRemoved = await _roomDal.SoftRemoveAsync(id);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda silinemedi")
                : Result<Room>.SuccessResult(roomToBeRemoved, "Oda silindi");
        }

        public async Task<Result<List<Room>>> RemoveRange(List<RemoveRoomDto> rooms)
        {
            var roomsToBeRemoved = _mapper.Map<List<Room>>(rooms);
            roomsToBeRemoved = _roomDal.RemoveRange(roomsToBeRemoved);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<List<Room>>.FailureResult("Odalar silinemedi")
                : Result<List<Room>>.SuccessResult(roomsToBeRemoved, "Odalar silindi");
        }

        public async Task<Result<Room>> Update(UpdateRoomDto room)
        {
            var roomToBeUpdated = _mapper.Map<Room>(room);
            roomToBeUpdated = _roomDal.Update(roomToBeUpdated);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda güncellenemedi")
                : Result<Room>.SuccessResult(roomToBeUpdated, "Oda güncellendi");

        }

        public async Task<Result<List<Room>>> UpdateRange(List<UpdateRoomDto> rooms)
        {
            var roomsToBeUpdated = _mapper.Map<List<Room>>(rooms);
            roomsToBeUpdated = _roomDal.UpdateRange(roomsToBeUpdated);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<List<Room>>.FailureResult("Odalar güncellenemedi")
                : Result<List<Room>>.SuccessResult(roomsToBeUpdated, "Odalar güncellendi");
        }

        public async Task<Result<Room>> RemoveAsyncById(int id)
        {
            var room = await _roomDal.GetByIdAsync(id);
            if (room == null)
                return Result<Room>.FailureResult("Oda bulunamadı");

            var result = _roomImageDal.RemoveAllByRoomId(id);
            var saved = 0;
            if (result)
            {
                saved = await _roomImageDal.SaveAsync();
                if (saved == 0)
                    return Result<Room>.FailureResult("Oda resimleri silinirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }

            room = _roomDal.Remove(room);
            saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda silinemedi")
                : Result<Room>.SuccessResult(room, "Oda silindi");
        }

        public async Task<Result<List<Room>>> RemoveRangeAsyncByHotelId(int hotelId)
        {
            var rooms = _roomDal.GetAll().Where(x => x.HotelId == hotelId).ToList();
            if (rooms == null || rooms.Count == 0)
                return Result<List<Room>>.FailureResult("Otele ait oda bulunamadı");

            foreach (var room in rooms)
            {
                var result = _roomImageDal.RemoveAllByRoomId(room.Id);
                var saved = 0;
                if (result)
                {
                    saved = await _roomImageDal.SaveAsync();
                    if (saved == 0)
                        return Result<List<Room>>.FailureResult("Oda resimleri silinirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
                }
            }

            var roomsToBeRemoved = _roomDal.RemoveRange(rooms);
            var savedRooms = await _roomDal.SaveAsync();
            return savedRooms == 0
                ? Result<List<Room>>.FailureResult("Odalar silinemedi")
                : Result<List<Room>>.SuccessResult(roomsToBeRemoved, "Odalar silindi");

        }
    }
}

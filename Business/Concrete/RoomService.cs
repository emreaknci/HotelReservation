using AutoMapper;
using Business.Abstract;
using Core.Entities;
using Core.Utils.Results;
using DataAccess.Abstract;
using Entities.Rooms;
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

        public RoomService(IRoomDal roomDal, IMapper mapper)
        {
            _roomDal = roomDal;
            _mapper = mapper;
        }

        public async Task<Result<Room>> AddAsync(CreateRoomDto room)
        {
            var newRoom = _mapper.Map<Room>(room);
            newRoom = await _roomDal.AddAsync(newRoom);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda eklenemedi")
                : Result<Room>.SuccessResult(newRoom, "Oda eklendi");
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

        public Result<List<Room>> GetRoomsByHotelId(int hotelId)
        {
            var rooms = _roomDal.GetAll().Where(x => x.HotelId == hotelId).ToList();
            return rooms == null || rooms.Count == 0
                ? Result<List<Room>>.FailureResult("Otele ait oda bulunamadı")
                : Result<List<Room>>.SuccessResult(rooms, "Odalar bulundu");

        }

        public async Task<Result<Room>> Remove(RemoveRoomDto room)
        {
            var roomToBeRemoved = _mapper.Map<Room>(room);
            roomToBeRemoved = await _roomDal.SoftRemoveAsync(roomToBeRemoved.Id);

            var saved = await _roomDal.SaveAsync();
            return saved == 0
                ? Result<Room>.FailureResult("Oda silinemedi")
                : Result<Room>.SuccessResult(roomToBeRemoved, "Oda silindi");
        }

        public async Task<Result<Room>> RemoveById(int id)
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
    }
}

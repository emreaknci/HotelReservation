using Core.Entities;
using Core.Utils.Results;
using Entities.Hotels;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRoomService
    {
        Result<List<Room>> GetAll();
        Task<Result<Room>> GetByIdAsync(int id);
        Result<List<Room>> GetRoomsByHotelId(int hotelId);
        Result<List<RoomDetailDto>> GetRoomsWithImagesByHotelId(int hotelId);
        Result<List<RoomDetailDto>> GetRoomsWithImages();
        Result<List<RoomDto>> GetLatestRoomsPerHotel(int? roomCount = null);
        Result<RoomDetailDto> GetByIdWithImages(int roomId);
        Task<Result<Room>> AddAsync(CreateRoomDto room);
        Task<Result<List<Room>>> AddRangeAsync(List<CreateRoomDto> rooms);
        Task<Result<Room>> UpdateAsync(UpdateRoomDto room);
        Task<Result<List<Room>>> UpdateRange(List<UpdateRoomDto> rooms);
        Task<Result<Room>> SoftRemoveAsync(RemoveRoomDto room);
        Task<Result<Room>> SoftRemoveAsyncById(int id);
        Task<Result<Room>> RemoveAsyncById(int id);
        Task<Result<List<Room>>> RemoveRangeAsyncByHotelId(int hotelId);
        Task<Result<List<Room>>> RemoveRange(List<RemoveRoomDto> rooms);
        Result<PaginationResult<Room>> GetAllPagination(BasePaginationRequest req);
        Task<Result<bool>> ChangeRoomStatus(int roomId);
    }
}

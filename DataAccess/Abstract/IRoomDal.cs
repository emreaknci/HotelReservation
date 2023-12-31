﻿using Core.DataAccess;
using Entities.Hotels;
using Entities.Rooms;
using System.Linq.Expressions;

namespace DataAccess.Abstract
{
    public interface IRoomDal : IGenericRepository<Room>
    {
        List<RoomDetailDto> GetRoomsWithImages(Expression<Func<RoomDetailDto, bool>> filter = null);
        List<RoomDetailDto> GetRoomsWithImagesByHotelId(int hotelId,Expression<Func<RoomDetailDto, bool>> filter = null);
        RoomDetailDto GetRoomWithImagesById(int id, Expression<Func<RoomDetailDto, bool>> filter = null);
        List<RoomDto> GetLatestRoomsPerHotel(int? roomCount = null, Expression<Func<RoomDto, bool>>? filter = null);

    }

}

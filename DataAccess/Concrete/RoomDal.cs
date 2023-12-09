using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Rooms;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class RoomDal : GenericRepository<Room, HotelReservationDbContext>, IRoomDal
    {
        public RoomDal(HotelReservationDbContext context) : base(context)
        {
            _context = context;
        }
        private readonly HotelReservationDbContext _context;


        public List<RoomDetailDto> GetRoomsWithImages(Expression<Func<RoomDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/no-image.jpg" };
            var result = from room in _context.Rooms
                         join roomImage in _context.RoomImages on room.Id equals roomImage.RoomId into roomImages
                         select new RoomDetailDto
                         {
                             Id = room.Id,
                             HotelId = room.HotelId,
                             Name = room.Name,
                             Description = room.Description,
                             Price = room.Price,
                             CreatedDate = room.CreatedDate,
                             UpdatedDate = room.UpdatedDate,
                             Images = roomImages.Any()
                                        ? roomImages.Select(image => "/Images/" + image.ImageUrl).ToList()
                                        : defaultImages,
                             Capacity = room.Capacity

                         };
            return filter == null
                ? result.ToList()
                : result.Where(filter).ToList();
        }

        public List<RoomDetailDto> GetRoomsWithImagesByHotelId(int hotelId, Expression<Func<RoomDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/no-image.jpg" };
            var result = from room in _context.Rooms
                         join roomImage in _context.RoomImages on room.Id equals roomImage.RoomId into roomImages
                         where room.HotelId == hotelId
                         select new RoomDetailDto
                         {
                             Id = room.Id,
                             HotelId = room.HotelId,
                             Name = room.Name,
                             Description = room.Description,
                             Price = room.Price,
                             CreatedDate = room.CreatedDate,
                             UpdatedDate = room.UpdatedDate,
                             Images = roomImages.Any()
                                        ? roomImages.Select(image => "/Images/" + image.ImageUrl).ToList()
                                        : defaultImages,
                             Capacity = room.Capacity,  
                             HotelName = room.Hotel.Name
                         };
            return filter == null
                ? result.ToList()
                : result.Where(filter).ToList();
        }

        public RoomDetailDto? GetRoomWithImagesById(int id, Expression<Func<RoomDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/defaultRoom.jpg" };
            var result = from room in _context.Rooms
                         join roomImage in _context.RoomImages on room.Id equals roomImage.RoomId into roomImages
                         where room.Id == id
                         select new RoomDetailDto
                         {
                             Id = room.Id,
                             HotelId = room.HotelId,
                             Name = room.Name,
                             Description = room.Description,
                             Price = room.Price,
                             CreatedDate = room.CreatedDate,
                             UpdatedDate = room.UpdatedDate,
                             Images = roomImages.Any()
                                        ? roomImages.Select(image => "/Images/" + image.ImageUrl).ToList()
                                        : defaultImages,
                             Capacity = room.Capacity,
                             HotelName = room.Hotel.Name

                         };

            return filter == null
                ? result.SingleOrDefault()
                : result.SingleOrDefault(filter);
        }


    }
}

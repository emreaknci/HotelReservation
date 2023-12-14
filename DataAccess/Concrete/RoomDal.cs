using Core.DataAccess;
using DataAccess.Abstract;
using Entities.HotelImages;
using Entities.Hotels;
using Entities.Rooms;
using System.Linq;
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
                             Capacity = room.Capacity,
                             HotelName = room.Hotel.Name,
                             IsDeleted = room.IsDeleted,
                             Status = room.Status,
                             Type = room.Type

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
                             HotelName = room.Hotel.Name,
                             IsDeleted = room.IsDeleted,
                             Status = room.Status,
                             Type = room.Type,
                         };
            return filter == null
                ? result.ToList()
                : result.Where(filter).ToList();
        }

        public RoomDetailDto? GetRoomWithImagesById(int id, Expression<Func<RoomDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/no-image.jpg" };
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

        public List<RoomDto> GetLatestRoomsPerHotel(int? roomCount = null, Expression<Func<RoomDto, bool>>? filter = null)
        {
            var result = from room in _context.Rooms
                         join hotel in _context.Hotels on room.HotelId equals hotel.Id
                         join roomImage in _context.RoomImages on room.Id equals roomImage.RoomId into roomImages
                         where room.Status == true && hotel.Status == true && roomImages.Any()
                         group new { Room = room, Images = roomImages } by room.HotelId into groupedRooms
                         select groupedRooms.Select(r => new RoomDto
                         {
                             Id = r.Room.Id,
                             HotelId = r.Room.HotelId,
                             Name = r.Room.Name,
                             Price = r.Room.Price,
                             ImagePath = "/Images/" + r.Images.OrderByDescending(img => img.Id).FirstOrDefault()!.ImageUrl,
                             Capacity = r.Room.Capacity,
                             HotelName = r.Room.Hotel.Name,
                             IsDeleted = r.Room.IsDeleted,
                             Status = r.Room.Status,
                         }).FirstOrDefault();

            result = filter == null ? result : result.Where(filter);

            result = (roomCount.HasValue && roomCount.Value > 0) ? result.Take(roomCount.Value) : result;

            return result.ToList();
        }
    }
}

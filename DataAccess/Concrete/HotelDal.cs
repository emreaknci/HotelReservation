using Core.DataAccess;
using DataAccess.Abstract;
using Entities.Hotels;
using System.Linq.Expressions;

namespace DataAccess.Concrete
{
    public class HotelDal : GenericRepository<Hotel, HotelReservationDbContext>, IHotelDal
    {

        public HotelDal(HotelReservationDbContext context) : base(context)
        {
            _context = context;
        }
        private readonly HotelReservationDbContext _context;


        public List<HotelDetailDto> GetHotelsWithImages(Expression<Func<HotelDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/no-image.jpg" };

            var result = from hotel in _context.Hotels
                         join hotelImage in _context.HotelImages on hotel.Id equals hotelImage.HotelId into hotelImages
                         select new HotelDetailDto
                         {
                             Id = hotel.Id,
                             Name = hotel.Name,
                             Description = hotel.Description,
                             Address = hotel.Address,
                             City = hotel.City,
                             Country = hotel.Country,
                             Star = hotel.Star,
                             CreatedDate = hotel.CreatedDate,
                             UpdatedDate = hotel.UpdatedDate,
                             Email = hotel.Email,
                             Phone = hotel.Phone,
                             IsDeleted = hotel.IsDeleted,
                             Website = hotel.Website,
                             Status = hotel.Status,
                             TotalRoomCount = hotel.TotalRoomCount,
                             Images = hotelImages.Any()
                                        ? hotelImages.Select(image => "/Images/" + image.ImageUrl).ToList()
                                        : defaultImages,
                         };

            return filter == null
                ? result.ToList()
                : result.Where(filter).ToList();
        }

        public HotelDetailDto? GetHotelWithImagesById(int id, Expression<Func<HotelDetailDto, bool>> filter = null)
        {
            var defaultImages = new List<string> { "/Images/no-image.jpg" };

            var result = from hotel in _context.Hotels
                         join hotelImage in _context.HotelImages on hotel.Id equals hotelImage.HotelId into hotelImages
                         where hotel.Id == id
                         select new HotelDetailDto
                         {
                             Id = hotel.Id,
                             Name = hotel.Name,
                             Description = hotel.Description,
                             Address = hotel.Address,
                             City = hotel.City,
                             Country = hotel.Country,
                             Star = hotel.Star,
                             CreatedDate = hotel.CreatedDate,
                             UpdatedDate = hotel.UpdatedDate,
                             Email = hotel.Email,
                             Phone = hotel.Phone,
                             IsDeleted = hotel.IsDeleted,
                             Website = hotel.Website,
                             Status = hotel.Status,
                             TotalRoomCount = hotel.TotalRoomCount,
                             Images = hotelImages.Any()
                                        ? hotelImages.Select(image => "/Images/" + image.ImageUrl).ToList()
                                        : defaultImages,
                         };

            return filter == null
                ? result.SingleOrDefault()
                : result.SingleOrDefault(filter);

            throw new NotImplementedException();
        }
    }

}

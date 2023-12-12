using Core.DataAccess;
using Core.Helpers;
using DataAccess.Abstract;
using Entities.HotelImages;
using Entities.Hotels;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class HotelImageDal : GenericRepository<HotelImage, HotelReservationDbContext>, IHotelImageDal
    {
        public HotelImageDal(HotelReservationDbContext context) : base(context)
        {
            Context = context;
        }

        private readonly HotelReservationDbContext Context;
        public bool RemoveAllByHotelId(int hotelId)
        {
            var hotelImages = Context.HotelImages.Where(x => x.HotelId == hotelId).ToList();
            if (hotelImages == null || hotelImages.Count == 0)
                return false;

            var imageUrls = hotelImages.Select(x => x.ImageUrl).ToList();

            RemoveRange(hotelImages);

            foreach (var imageUrl in imageUrls)
            {
                var result = FileHelper.Delete(imageUrl);
                if (!result.Success)
                    return false;
            }

            return true;
        }

        public bool RemoveRange(List<string> imagePaths)
        {
            imagePaths = imagePaths.Select(x => x.Replace("/Images/", "")).ToList();
            if (imagePaths == null || !imagePaths.Any())
                return false;
            var images = Context.HotelImages
                .Where(x => imagePaths.Contains(x.ImageUrl)).ToList();
            if (images == null || images.Count == 0)
                return false;

            RemoveRange(images);

            foreach (var imagePath in images)
            {
                var result = FileHelper.Delete(imagePath.ImageUrl);
                if (!result.Success)
                    return false;
            }

            return true;
        }
    }
}

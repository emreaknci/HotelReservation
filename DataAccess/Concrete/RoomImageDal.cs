using Core.DataAccess;
using Core.Helpers;
using DataAccess.Abstract;
using Entities.RoomImages;

namespace DataAccess.Concrete
{
    public class RoomImageDal : GenericRepository<RoomImage, HotelReservationDbContext>, IRoomImageDal
    {
        public RoomImageDal(HotelReservationDbContext context) : base(context)
        {
            Context = context;
        }
        private readonly HotelReservationDbContext Context;
        public bool RemoveAllByRoomId(int roomId)
        {
            var roomImages = Context.RoomImages.Where(x => x.RoomId == roomId).ToList();
            if (roomImages == null || roomImages.Count==0)
                return false;

            var imageUrls = roomImages.Select(x => x.ImageUrl).ToList();

            RemoveRange(roomImages);

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
            var images = Context.RoomImages
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

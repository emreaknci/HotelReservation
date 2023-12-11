using Core.DataAccess;
using Entities.HotelImages;
using Entities.Hotels;
using Entities.RoomImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IHotelImageDal : IGenericRepository<HotelImage>
    {
        bool RemoveAllByHotelId(int hotelId);
    }
}

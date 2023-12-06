using Core.DataAccess;
using DataAccess.Abstract;
using Entities.HotelImages;
using Entities.Hotels;
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
        }
    }
}

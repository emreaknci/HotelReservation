using Core.DataAccess;
using Core.Entities;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class UserDal : GenericRepository<AppUser, HotelReservationDbContext>, IUserDal
    {
        public UserDal(HotelReservationDbContext context) : base(context)
        {
        }
    }
}

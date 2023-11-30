using Core.DataAccess;
using Core.Entities;
using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IHotelDal : IGenericRepository<Hotel>
    {
    }
}

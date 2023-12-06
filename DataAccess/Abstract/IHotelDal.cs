using Core.DataAccess;
using Core.Entities;
using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IHotelDal : IGenericRepository<Hotel>
    {
        List<HotelDetailDto> GetHotelsWithImages(Expression<Func<HotelDetailDto, bool>> filter = null);
        HotelDetailDto GetHotelWithImagesById(int id, Expression<Func<HotelDetailDto, bool>> filter = null);
    }
}

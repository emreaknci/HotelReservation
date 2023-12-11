﻿using Core.DataAccess;
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
            if (hotelImages == null || hotelImages.Count==0)
                return false;
            RemoveRange(hotelImages);
            return true;
        }
    }
}

using Core.Entities;
using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.HotelImages
{
    public class HotelImage:BaseEntity
    {
        public string? ImageUrl { get; set; }
        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}

using Core.Entities;
using Entities.Hotels;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RoomImages
{
    public class RoomImage : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public int? RoomId { get; set; }
        public Room? Room{ get; set; }
    }
}

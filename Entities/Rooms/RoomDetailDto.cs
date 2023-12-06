using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Rooms
{
    public record RoomDetailDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public RoomType Type { get; set; }
        public int? Capacity { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public List<string>? Images { get; set;}
    }
}

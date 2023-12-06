using Entities.Reservations;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Hotels
{
    public record HotelDetailDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public int? Star { get; set; }
        public int? TotalRoomCount { get; set; }
        public List<string> Images { get; set; }
        public ICollection<Room>? Rooms { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}

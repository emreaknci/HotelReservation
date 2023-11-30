using Entities.Reservations;
using Entities.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Hotels
{
    public class Hotel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Room>? Rooms { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }

}

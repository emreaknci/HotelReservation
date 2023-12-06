using Entities.HotelImages;
using Microsoft.AspNetCore.Http;

namespace Entities.Hotels
{
    public record CreateHotelDto
    {
        public string? Name { get; init; }
        public string? Address { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }
        public string? Phone { get; init; }
        public string? Email { get; init; }
        public string? Website { get; init; }
        public string? Description { get; init; }
        public List<IFormFile>? Images{ get; init; }
        public int? Star { get; init; }
        public int? TotalRoomCount { get; init; }
    }
}

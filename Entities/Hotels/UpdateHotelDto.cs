using Microsoft.AspNetCore.Http;

namespace Entities.Hotels
{
    public record UpdateHotelDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Address { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }
        public string? Phone { get; init; }
        public string? Email { get; init; }
        public string? Website { get; init; }
        public string? Description { get; init; }
        public int? Star { get; init; }
        public int? TotalRoomCount { get; init; }
        public List<IFormFile>? NewImages{ get; set; }
        public List<string>?  ImagePathsToDelete{ get; set; }
    }
}

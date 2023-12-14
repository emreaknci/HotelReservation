namespace Entities.Rooms
{
    public record RoomDto
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? Capacity { get; set; }
        public decimal? Price { get; set; }
        public string? Name { get; set; }
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? ImagePath { get; set; }

    }
}

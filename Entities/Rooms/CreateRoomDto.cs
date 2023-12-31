﻿using Microsoft.AspNetCore.Http;

namespace Entities.Rooms
{
    public record CreateRoomDto
    {
        public RoomType? Type { get; init; }
        public int? Capacity { get; init; }
        public int? Price { get; init; }
        public string? Description { get; init; }
        public string? Name { get; init; }
        public int? HotelId { get; init; }
        public List<IFormFile>? Images { get; init; }
    }
}

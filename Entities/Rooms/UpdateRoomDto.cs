﻿using Microsoft.AspNetCore.Http;

namespace Entities.Rooms
{
    public record UpdateRoomDto
    {
        public int? Id { get; set; }
        public int? RoomId { get; init; }
        public RoomType? Type { get; init; }
        public int? Capacity { get; init; }
        public int? Price { get; init; }
        public string? Description { get; init; }
        public string? Name { get; init; }
        public int? HotelId { get; init; }
        public List<IFormFile>? NewImages { get; set; }
        public List<string>? ImagePathsToDelete { get; set; }
    }
}

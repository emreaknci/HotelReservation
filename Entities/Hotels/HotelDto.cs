﻿namespace Entities.Hotels
{
    public record HotelDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Status{ get; set; }

    }
}

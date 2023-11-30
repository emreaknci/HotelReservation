﻿namespace Core.Entities
{
    public class BasePaginationRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

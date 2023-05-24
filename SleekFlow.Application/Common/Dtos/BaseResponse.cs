﻿using SleekFlow.Application.Mappings;

namespace SleekFlow.Application.Common.Dtos
{
    public abstract class BaseResponse<T> : IMapFrom<T>
    {
        public string Id { get; set; } = string.Empty;

        public DateTime AddAt { get; set; }

        public string AddBy { get; set; } = string.Empty;

        public DateTime EditAt { get; set; }

        public string EditBy { get; set; } = string.Empty;
    }
}

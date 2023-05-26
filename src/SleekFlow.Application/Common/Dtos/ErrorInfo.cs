﻿namespace SleekFlow.Application.Common.Dtos
{
    public class ErrorInfo
    {
        public int Code { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}

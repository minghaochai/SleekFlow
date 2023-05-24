namespace SleekFlow.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string type, string message)
            : base(message)
        {
            Type = type;
        }

        public BadRequestException(string type, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }

        public string Type { get; set; } = "common.badRequest";
    }
}

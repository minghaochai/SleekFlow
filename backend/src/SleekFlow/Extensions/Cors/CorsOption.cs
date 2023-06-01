namespace SleekFlow.Api.Extensions.Cors
{
    public class CorsOptions
    {
        public const string Cors = "Cors";

        public string Origins { get; set; } = string.Empty;

        public string[] GetOrigins()
        {
            return Origins?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>().ToArray();
        }
    }
}

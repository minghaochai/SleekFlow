using Newtonsoft.Json;
using System.Text;

namespace Helper.Http
{
    public static class Http
    {
        public static HttpContent CastToHttpContent<T>(this T model)
        {
            var json = JsonConvert.SerializeObject(model);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            return data;
        }

        public static T CastToModel<T>(this HttpResponseMessage response)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            var finalResponse = JsonConvert.DeserializeObject<T>(result);
            return finalResponse;
        }
    }
}

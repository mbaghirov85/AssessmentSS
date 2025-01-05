using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace assessment_platform_developer.Helpers {

    public class HttpBodyHelper {

        public async Task<string> GetBodyMessage(HttpResponseMessage response) {
            try {
                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                return (string)json["Message"];
            } catch (Exception) {
                return "";
            }
        }
    }
}
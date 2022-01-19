using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserInterface
{
    public class Balaboba
    {
        class Response
        {
            public int bad_query { get; set; }
            public int error { get; set; }
            public string query { get; set; }
            public string text { get; set; }
        }

        class Message
        {
            public int filter { get; set; }
            public int intro { get; set; }
            public string query { get; set; }
        }
        
        public async Task<string> GetAnswer(string text, int style = 0)
        {
            var result = text + " ";
            var message = new Message { filter = 1, intro = style, query = text };
            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync("https://zeapi.yandex.net/lab/api/yalm/text3", content);
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<Response>(responseContent);
                    result += res.text;
                }
            }
            return result;
        }
    }
}
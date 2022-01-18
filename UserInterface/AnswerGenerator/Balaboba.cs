using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UserInterface
{
    public class Balaboba
    {
        private class Response
        {
            public int BadQuery { get; set; }
            public int Error { get; set; }
            public string Query { get; set; }
            public string Text { get; set; }
        }

        private class Message
        {
            public int Filter { get; set; }
            public int Intro { get; set; }
            public string Query { get; set; }
        }
        
        public async Task<string> GetAnswer(string text, int style = 11)
        {
            var result = text + " ";
            var message = new Message { Filter = 1, Intro = style, Query = text };
            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var httpClient = new HttpClient();
            var httpResponse = await httpClient.PostAsync("https://zeapi.yandex.net/lab/api/yalm/text3", content);
            if (httpResponse.Content == null) return result;
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Response>(responseContent);
            result += res.Text;

            return result;
        }
    }
}
using Medinova.Consts;
using Medinova.DTOs;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Medinova.Services
{
    public class AiHealthSuggestionService
    {
        private readonly string _apiKey;

        public AiHealthSuggestionService()
        {
            _apiKey = ConfigurationManager.AppSettings["OpenAI_ApiKey"];
        }

        public async Task<AiDepartmentResultDto> GetDepartmentSuggestionAsync(string complaint)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                    new { role = "system", content = MedinovaAiPrompt.SystemPrompt },
                    new { role = "user", content = MedinovaAiPrompt.BuildUserPrompt(complaint) }
                },
                    temperature = 0.2
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content
                );

                var responseJson = await response.Content.ReadAsStringAsync();

                dynamic parsed = JsonConvert.DeserializeObject(responseJson);
                string aiText = parsed.choices[0].message.content.ToString();

                aiText = aiText
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                return JsonConvert.DeserializeObject<AiDepartmentResultDto>(aiText);
            }
        }
    }
}

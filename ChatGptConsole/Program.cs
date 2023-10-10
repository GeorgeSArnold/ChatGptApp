using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length > 0)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer APITOKEN HERE!");

            string prompt = args[0];
            string payload = JsonConvert.SerializeObject(new
            {
                model = "text-davinci-001",
                prompt,
                temperature = 1.0,
                max_tokens = 100
            });

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

            string responseString = await response.Content.ReadAsStringAsync();

            // deserialize response dynamic data > obj
            try
            {
                var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

                Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine($"---> Api response: {dyData!.choices[0].text}"); // can be null + obj choices > prop: text

            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"---> cant deserialize JSON > obj: {ex.Message}");
            }

            //Console.WriteLine(responseString);
        }
        else
        {
            Console.WriteLine("---> need input!");
        }
    }
}

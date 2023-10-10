using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
            client.DefaultRequestHeaders.Add("Authorization", "Bearer APIKEYHERE");

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

                string guess = GuessCommad(dyData!.choices[0].text);
                Console.ForegroundColor = ConsoleColor.DarkCyan;


                System.Console.WriteLine($"---> Api answer: {guess}"); // can be null + obj choices > prop: text

                Console.ForegroundColor = ConsoleColor.Yellow;

            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"---> cant deserialize JSON > obj: {ex.Message}");

                            Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("---> need input!");
        }

        // save guess > clipboard
        static string GuessCommad(string raw)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("GPT-3 API Returned Text:");
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.ResetColor();
            //System.Console.WriteLine(raw);

            var lastIndex = raw.LastIndexOf('\n');
            string guess = raw.Substring(lastIndex +1);

                        Console.ResetColor();

            TextCopy.ClipboardService.SetText(guess);

            return guess;
        }
    }
}

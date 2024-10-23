using NetworkTask3.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class HttpClientExample
{
    static async Task Main(string[] args)
    {
        string baseUrl = "http://localhost:27001/";

        Console.WriteLine("1.All Users");
        Console.WriteLine("2.Add User");
        Console.WriteLine("3.Delete User");
        Console.Write("Secimi daxil et:");
        int secim = int.Parse(Console.ReadLine());
        try
        {
            switch (secim)
            {
                case 1:
                    {
                        await GetUsers(baseUrl);
                        break;
                    }

                case 2:
                    {
                        Console.Write("Ad: ");
                        var Name = Console.ReadLine();
                        Console.Write("Email: ");
                        var Email = Console.ReadLine();
                        Console.Write("Yas: ");
                        var Age = int.Parse(Console.ReadLine());

                        var newUser = new User
                        {
                            Name = Name!,
                            Email = Email!,
                            Age = Age
                        };
                        await AddUser(baseUrl, newUser);
                        break;
                    }
                case 3:
                    {
                        int userIdToDelete = 2;
                        await DeleteUser(baseUrl, userIdToDelete);


                        break;
                    }
                default:
                    {
                        Console.WriteLine("Secim yanlis daxil edilmisdir:");
                        break;
                    }
            }

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }
    }

    static async Task GetUsers(string baseUrl)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine(json);
            }
            else
            {
                Console.WriteLine("Users tapilmadi:" + response.StatusCode);
            }
        }
    }

    static async Task AddUser(string baseUrl, User newUser)
    {
        using (var client = new HttpClient())
        {
            string json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(baseUrl, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("User ugurla elave edildi");
            }
            else
            {
                Console.WriteLine("User elave edilmedi: " + response.StatusCode);
            }
        }
    }

    static async Task DeleteUser(string baseUrl, int userId)
    {
        using (var client = new HttpClient())
        {

            string json = JsonSerializer.Serialize(userId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(baseUrl),
                Content = content
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("user silindi.");
            }
            else
            {
                Console.WriteLine("User silinmedi: " + response.StatusCode);
            }
        }
    }
}

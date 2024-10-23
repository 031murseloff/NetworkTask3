using Azure.Core;
using NetworkTask3.Contexts;
using NetworkTask3.Models;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

var listener = new HttpListener();
var port = 27001;
listener.Prefixes.Add($"http://localhost:{port}/"); // 127.0.0.1
listener.Start();

Console.WriteLine($"Http server started on {port}");


while (true)
{
    var context = listener.GetContext();
    _ = Task.Run(() => { HandleRuquest(context); });
}

async void HandleRuquest(HttpListenerContext context)
{
    if (context.Request.HttpMethod == "GET")
    {


        var stream = context.Response.OutputStream;


        var UserList = (await GetAlluser()).ToArray();
        var bytes = new byte[UserList.Length];


        string userListJson = JsonSerializer.Serialize(UserList);
        bytes = System.Text.Encoding.UTF8.GetBytes(userListJson);
        await stream.WriteAsync(bytes, 0, bytes.Length);





        stream.Close();
    }

    if (context.Request.HttpMethod == "DELETE")
    {

    }



    if (context.Request.HttpMethod == "POST")
    {
        string requestData;
        using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            requestData = reader.ReadToEnd();
        }
        var dataObject = JsonSerializer.Deserialize<User>(requestData);
        await AddToDatabaseAsync(dataObject!);
    }
}

static async Task DeleteUser(int id)
{
    using (var dbContext = new UserDbContext())
    {


        dbContext.Users.FirstOrDefault(x => x.Id == id);

        dbContext.SaveChanges();

    }

}

static async Task<List<User>> GetAlluser()
{
    var users = new List<User>();
    using (var dbContext = new UserDbContext())
    {


        users = dbContext.Users.ToList();

    }

    return users;
}


static async Task AddToDatabaseAsync(User data)
{

    using (var dbContext = new UserDbContext())
    {


        dbContext.Users.Add(data);
        await dbContext.SaveChangesAsync();
    }
}
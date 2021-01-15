using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MyData.Core.Data;
using MyData.Core.Extensions;
using MyData.Data;
using MyData.Data.Domain;

namespace MyData.Console {
    public class Program {
        public static async Task Main(string[] args) {
            System.Console.WriteLine("Hello World!\n");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var connectionString = configuration.GetConnectionString("MyDbContext");
            var context = new MyDbContext(connectionString);
            var cityRepository = new Repository<City, MyDbContext>(context);

            AddConsoleLog("Insert...");
            var logUserId = Guid.NewGuid();
            var randomText = logUserId.ToString();
            var newCity = new City {
                Code = randomText.Replace("-", string.Empty).Substring(0, 10),
                Name = randomText
            };
            var id = await cityRepository.AddUpdate(newCity, logUserId);
            AddConsoleLog($"Inserted Id:{id}\n");

            AddConsoleLog("Find by query...");
            var city = await cityRepository.Find(f => f.Id == id);
            AddConsoleLog($"Find result:{city.SerializeToJson()}\n");

            AddConsoleLog("Update...");
            logUserId = Guid.NewGuid();
            randomText = logUserId.ToString();
            city.Code = randomText.Replace("-", string.Empty).Substring(0, 10);
            city.Name = randomText;
            await cityRepository.AddUpdate(city, logUserId);
            AddConsoleLog($"Updated\n");

            AddConsoleLog("Filter by query...");
            var list = await cityRepository.Filter(f => f.Id == id).ToListAsync();
            AddConsoleLog($"Filter result:{list.SerializeToJson()}\n");

            AddConsoleLog("Filter by query...");
            var list2 = cityRepository.Filter(f => f.Id == id, 0, 20, f => f.CreatedDateTime, true);
            var list2Total = list2.Total;
            var list2Data = await list2.Data.ToListAsync();
            AddConsoleLog($"Filter result-Total:{list2Total}");
            AddConsoleLog($"Filter result-Data:{list2Data.SerializeToJson()}\n");

            AddConsoleLog("Detail by id...");
            var detail = await cityRepository.Detail(id);
            AddConsoleLog($"Detail result:{detail.SerializeToJson()}\n");

            AddConsoleLog("Delete by id...");
            var delete = await cityRepository.Delete(id, logUserId);
            AddConsoleLog($"Delete result:{delete}\n");

            AddConsoleLog("HardDelete by id...");
            var hardDelete = await cityRepository.HardDelete(id);
            AddConsoleLog($"HardDelete result:{hardDelete}\n");
        }

        private static void AddConsoleLog(string msg) {
            System.Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.ffff}-{msg}");
        }
    }
}

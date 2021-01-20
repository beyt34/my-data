using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MyData.Core.Data;
using MyData.Core.Extensions;
using MyData.Data;
using MyData.Data.Domain;

namespace MyData.Console {
    public class Program {
        private static string connectionString;

        public static async Task Main(string[] args) {
            System.Console.WriteLine("Hello World!\n");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            connectionString = configuration.GetConnectionString("MyDbContext");

            await CrudOperation();
            await BulkInsert();
        }

        private static async Task BulkInsert() {
            var cities = new List<City>();

            AddConsoleLog("Started Preparing Bulk Data...");
            for (var i = 0; i < 100000; i++) {
                var guid = Guid.NewGuid();
                var city = new City {
                    Id = guid,
                    Code = (i + 1).ToString("00000#"),
                    Name = $"Name-{(i + 1):00000#}",
                    CreatedDateTime = DateTime.UtcNow,
                    CreatedUserId = guid
                };

                cities.Add(city);
            }

            AddConsoleLog("Ended Preparing Bulk Data...");
            AddConsoleLog("Started Insert Bulk Data...");

            var context = new MyDbContext(connectionString);
            var cityRepository = new Repository<City, MyDbContext>(context);
            await cityRepository.BulkInsertAsync(cities);

            AddConsoleLog("Ended Insert Bulk Data...");
        }

        private static async Task CrudOperation() {
            var context = new MyDbContext(connectionString);
            var cityRepository = new Repository<City, MyDbContext>(context);

            AddConsoleLog("Insert...");
            var logUserId = Guid.NewGuid();
            var randomText = logUserId.ToString();
            var newCity = new City {
                Code = randomText.Replace("-", string.Empty).Substring(0, 10),
                Name = randomText
            };
            var id = await cityRepository.AddUpdateAsync(newCity, logUserId);
            AddConsoleLog($"Inserted Id:{id}\n");

            AddConsoleLog("DetailAsync by id...");
            var detail = await cityRepository.DetailAsync(id);
            AddConsoleLog($"DetailAsync result:{detail.SerializeToJson()}\n");

            AddConsoleLog("Update...");
            logUserId = Guid.NewGuid();
            randomText = logUserId.ToString();
            detail.Code = randomText.Replace("-", string.Empty).Substring(0, 10);
            detail.Name = randomText;
            await cityRepository.AddUpdateAsync(detail, logUserId);
            AddConsoleLog($"Updated\n");

            AddConsoleLog("FilterAsync by query...");
            var list = await cityRepository.FilterAsync(f => f.Id == id).ToListAsync();
            AddConsoleLog($"FilterAsync result:{list.SerializeToJson()}\n");

            AddConsoleLog("FilterAsync by query...");
            var list2 = cityRepository.FilterAsync(f => f.Id == id, 0, 20, f => f.CreatedDateTime, true);
            var list2Total = list2.Total;
            var list2Data = await list2.Data.ToListAsync();
            AddConsoleLog($"FilterAsync result-Total:{list2Total}");
            AddConsoleLog($"FilterAsync result-Data:{list2Data.SerializeToJson()}\n");

            AddConsoleLog("DeleteAsync by id...");
            var delete = await cityRepository.DeleteAsync(id, logUserId);
            AddConsoleLog($"DeleteAsync result:{delete}\n");

            AddConsoleLog("HardDeleteAsync by id...");
            var hardDelete = await cityRepository.HardDeleteAsync(id);
            AddConsoleLog($"HardDeleteAsync result:{hardDelete}\n");
        }

        private static void AddConsoleLog(string msg) {
            System.Console.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.ffff}-{msg}");
        }
    }
}

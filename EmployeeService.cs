
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeWorkingHours
{
    public class EmployeeService
    {
        public async Task<List<Employee>> GetEmployees()
        {
            HttpClient client = new HttpClient();

            string url = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

            string json = await client.GetStringAsync(url);

            var employees = JsonSerializer.Deserialize<List<Employee>>(json);

            var result = employees
                .GroupBy(e => e.EmployeeName)
                .Select(g => new
                {
                    Name = g.Key,
                    TotalHours = g.Sum(e =>
                        (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                })
                .OrderByDescending(e => e.TotalHours)
                .ToList();

            foreach (var item in result)
            {
                Console.WriteLine("Name: " + item.Name);
                Console.WriteLine("Total Hours: " + item.TotalHours);
            }

            return employees ?? new List<Employee>();
        }
    }
}

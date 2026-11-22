
using System;
using System.Threading.Tasks;

namespace EmployeeWorkingHours
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            EmployeeService service = new EmployeeService();

            var employees = await service.GetEmployees();

            Console.WriteLine("Employee data loaded successfully!");

            Console.WriteLine("Total Records: " + employees.Count);

            HtmlGenerator generator = new HtmlGenerator();

            generator.GenerateHtml(employees);

            Console.ReadLine();
        }
    }
}

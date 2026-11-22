
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWorkingHours
{
    public class HtmlGenerator
    {
        public void GenerateHtml(List<Employee> employees)
        {
            string html = "";

            html += "<html>";
            html += "<head>";
            html += "<title>Employee Working Hours</title>";

            html += "<style>";

            html += "body { font-family: Arial; margin: 30px; }";

            html += "table { border-collapse: collapse; width: 70%; }";

            html += "th, td { border: 1px solid black; padding: 10px; }";

            html += "th { background-color: #333; color: white; }";

            html += ".low { background-color: #ffcccc; }";

            html += "</style>";

            html += "</head>";
            html += "<body>";

            html += "<h2>Employee Working Hours</h2>";

            // Calculate total hours for each employee

            List<string> names = new List<string>();

            List<double> hoursList = new List<double>();

            foreach (Employee emp in employees)
            {
                int index = names.IndexOf(emp.EmployeeName);

                double hours = (emp.EndTimeUtc - emp.StarTimeUtc).TotalHours;

                if (index == -1)
                {
                    names.Add(emp.EmployeeName);

                    hoursList.Add(hours);
                }
                else
                {
                    hoursList[index] += hours;
                }
            }

            // Calculate total working hours

            double total = 0;

            foreach (double hours in hoursList)
            {
                if (hours > 0)
                {
                    total += hours;
                }
            }

            // Pie Chart

            html += "<h2>Employee Working Hours Pie Chart</h2>";

            html += "<div style='display:flex; align-items:center; gap:30px;'>";

            html += "<svg width='350' height='350' viewBox='0 0 350 350'>";

            string[] colors =
            {
                "red", "blue", "green", "orange",
                "purple", "yellow", "pink", "cyan"
            };

            double angle = -90;

            if (total > 0)
            {
                for (int i = 0; i < hoursList.Count; i++)
                {
                    if (hoursList[i] <= 0)
                    {
                        continue;
                    }

                    double percentage = hoursList[i] / total;

                    double sweep = percentage * 360;

                    double startX = 175 + 130 *
                        Math.Cos(angle * Math.PI / 180);

                    double startY = 175 + 130 *
                        Math.Sin(angle * Math.PI / 180);

                    angle += sweep;

                    double endX = 175 + 130 *
                        Math.Cos(angle * Math.PI / 180);

                    double endY = 175 + 130 *
                        Math.Sin(angle * Math.PI / 180);

                    int largeArc = sweep > 180 ? 1 : 0;

                    html += "<path d='M175 175 L" +
                        startX + " " + startY +
                        " A130 130 0 " + largeArc + " 1 " +
                        endX + " " + endY +
                        " Z' fill='" +
                        colors[i % colors.Length] + "'/>";
                }
            }

            html += "</svg>";

            // Pie Chart Employee Details

            html += "<div>";

            html += "<h3>Employee Details</h3>";

            for (int i = 0; i < names.Count; i++)
            {
                if (hoursList[i] <= 0)
                {
                    continue;
                }

                double percentage = (hoursList[i] / total) * 100;

                html += "<p style='color:" +
                    colors[i % colors.Length] + "'>";

                html += System.Net.WebUtility.HtmlEncode(names[i]);

                html += " - ";

                html += hoursList[i].ToString("F2");

                html += " Hours - ";

                html += percentage.ToString("F2") + "%";

                html += "</p>";
            }

            html += "</div>";

            html += "</div>";

            // Employee Table

            html += "<h2>Employee Details</h2>";

            html += "<table>";

            html += "<tr>";

            html += "<th>Employee Name</th>";

            html += "<th>Total Hours</th>";

            html += "</tr>";

            for (int i = 0; i < names.Count; i++)
            {
                string rowClass = hoursList[i] < 100 ? "low" : "";

                html += "<tr class='" + rowClass + "'>";

                html += "<td>";

                html += System.Net.WebUtility.HtmlEncode(names[i]);

                html += "</td>";

                html += "<td>";

                html += hoursList[i].ToString("F2");

                html += "</td>";

                html += "</tr>";
            }

            html += "</table>";

            html += "</body>";

            html += "</html>";

            Directory.CreateDirectory("Outputs");

            

            string path = "Outputs/index.html";

            File.WriteAllText(path, html);

            Console.WriteLine("HTML generated successfully!");

            Console.WriteLine(Path.GetFullPath(path));
        }
    }
}

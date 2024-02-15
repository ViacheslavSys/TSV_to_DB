using System;
using System.IO;
using System.Linq;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class EmployeeImporter
    {
        public void ImportEmployee(string fileName)
        {
            using var bd = new BdContext();

            var lines = File.ReadLines(fileName).Skip(1); // Ленивая загрузка строк из файла
            var employeesInBD = bd.Employees.ToList();
            foreach (var line in lines)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(line) && line != "\t\t\t\t\t")
                    {
                        var value = line.Split('\t').Select(s => s.Trim()); // Убираем лишние пробелы

                        value = value.Select((s, i) =>
                            i switch
                            {
                                0 => StringHelper.FirstUpper(s, false).Replace("  ", " "), // Первая буква в верхний регистр
                                1 => StringHelper.FirstUpper(s, true),
                                _ => s
                            });

                        var e = new Employee
                        {
                            Departament = bd.Departaments.FirstOrDefault(dept => dept.Name == value.ElementAt(0))?.Id,
                            Fullname = value.ElementAt(1),
                            Login = value.ElementAt(2),
                            Password = value.ElementAt(3),
                            Jobtitleid = bd.Jobtitles.FirstOrDefault(job => job.Name == value.ElementAt(4))?.Id
                        };

                        var employeeUpdate = bd.Employees.FirstOrDefault(emp => emp.Fullname == e.Fullname);
                        if (employeeUpdate != null)
                        {
                            employeeUpdate.Departament = e.Departament;
                            employeeUpdate.Password = e.Password;
                            employeeUpdate.Jobtitleid = e.Jobtitleid;
                            bd.Employees.Update(employeeUpdate);
                        }
                        else
                        {
                            var employeeNew = new Employee
                            {
                                Fullname = e.Fullname,
                                Departament = e.Departament,
                                Login = e.Login,
                                Password = e.Password,
                                Jobtitleid = e.Jobtitleid
                            };
                            bd.Employees.Add(employeeNew);
                        }
                        bd.SaveChanges();
                    }
                }
                catch
                {
                    Console.WriteLine("stderror");
                }
            }
        }
    }
}

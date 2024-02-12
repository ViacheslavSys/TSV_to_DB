using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class EmployeeImporter
    {
        public void ImportEmployee(string fileName)
        {
            using var bd = new BdContext();

            var list = File.ReadAllLines(fileName).Skip(1);
            var employeesInBD = bd.Employees.ToList();
            List<Employee> employees = new List<Employee>();
            foreach (var line in list)
            {
                try
                {
                    if (line != "\t\t\t\t\t")
                    {
                        var value = line.Split('\t');

                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = value[i].TrimStart(' ');
                            value[i] = value[i].TrimEnd(' ');
                            value[i] = value[i].Replace("  ", " ");
                        }
                        value[0] = StringHelper.FirstUpper(value[0], false);
                        value[1] = StringHelper.FirstUpper(value[1], true);
                        value[4] = StringHelper.FirstUpper(value[4]);
                        Employee e = new Employee();

                        e.Departament = (from Departament in bd.Departaments
                                         where Departament.Name == value[0]
                                         select Departament.Id).FirstOrDefault();

                        e.Fullname = value[1];
                        e.Login = value[2];
                        e.Password = value[3];

                        e.Jobtitleid = (from Jobtitle in bd.Jobtitles
                                        where Jobtitle.Name == value[4]
                                        select Jobtitle.Id).FirstOrDefault();

                        Employee employee_update = bd.Employees.FirstOrDefault(s => s.Fullname == e.Fullname);
                        if (employee_update != null)
                        {
                            employee_update.Departament = e.Departament;
                            employee_update.Password = e.Password;
                            employee_update.Jobtitleid = e.Jobtitleid;
                            bd.Employees.Update(employee_update);
                        }
                        else
                        {
                            Employee employee_new = new Employee()
                            {
                                Fullname = e.Fullname,
                                Departament = e.Departament,
                                Password = e.Password,
                                Jobtitleid = e.Jobtitleid

                            };
                            bd.Employees.Add(employee_new);
                        }
                        bd.SaveChanges();
                    }
            }
                catch { Console.WriteLine("stderror"); }

        }

        }
    }
}

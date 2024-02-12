using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB
{
    internal static class ShowCommand
    {
        public static void Departments()
        {
            using var bd = new BdContext();
            var departaments = bd.Departaments.OrderBy(dep => dep.Name);
            Console.WriteLine("\nDepartaments:");

            var parent = (from Departament in bd.Departaments
                          where Departament.Parentid == null
                          select Departament).ToList();
            List<Departament> list = parent;
            string f = "";
            string space = "";
            List<string> ls = Recursive(bd, list, f, space);
            foreach (string de in ls)
            {
                Console.WriteLine(de);
            }
        }
        static List<string> Recursive(BdContext bd, List<Departament> list_departaments, string f, string space)
        {
            List<string> ls = new List<string>();
            f = f + "=";
            space = space.PadLeft(f.Length - 1, ' ');
            var jobtitle = "";
            var manager = "";
            try
            {
                for (var i = 0; i < list_departaments.Count; i++)
                {
                    ls.Add($"{f}{list_departaments[i].Name} ID={list_departaments[i].Id}");

                    manager = (from Employee in bd.Employees
                               where Employee.Id == list_departaments[i].Managerid
                               select Employee.Fullname).FirstOrDefault();
                    var jobid = (from Employee in bd.Employees
                                 where Employee.Id == list_departaments[i].Managerid
                                 select Employee.Jobtitleid).FirstOrDefault();
                    jobtitle = (from Jobtitle in bd.Jobtitles
                                where Jobtitle.Id == jobid
                                select Jobtitle.Name).FirstOrDefault();
                    if (manager != null && jobid != null && jobtitle != null)
                    {
                        ls.Add($"{space}*{manager} ID={list_departaments[i].Managerid} ({jobtitle} ID={jobid})");
                    }

                    var employee = (from Employee in bd.Employees
                                    where Employee.Departament == list_departaments[i].Id
                                    select Employee).ToList();
                    foreach (Employee emp in employee)
                    {
                        if (emp.Fullname == manager)
                        {
                            continue;
                        }
                        jobtitle = (from Jobtitle in bd.Jobtitles
                                    where Jobtitle.Id == emp.Jobtitleid
                                    select Jobtitle.Name).FirstOrDefault();
                        if (jobtitle != null)
                        {
                            ls.Add($"{space}-{emp.Fullname} ID={emp.Id} ({jobtitle} ID={emp.Jobtitleid})");
                        }
                    }


                    try
                    {
                        long id = list_departaments[i].Id;
                        var depart = (from Departament in bd.Departaments
                                      where Departament.Parentid == id
                                      select Departament).ToList();
                        List<Departament> list = depart;
                        ls.AddRange(Recursive(bd, list, f, space));
                    }
                    catch { }
                }
            }
            catch { }

            return ls;
        }
        public static void DepartmentByID(long id)
        {
            using var bd = new BdContext();

            var search_departament = (from Departament in bd.Departaments
                                      where Departament.Id == id
                                      select Departament).FirstOrDefault();

            if (search_departament != null)
            {
                string f = "";
                string space = "";
                List<string> lsID = ShowDepartamentByID(bd, search_departament);
                lsID = EandM(bd, search_departament, lsID);


                foreach (string de in lsID)
                {
                    Console.WriteLine(de);
                }
            }
        }
        static List<string> ShowDepartamentByID(BdContext bd, Departament d)
        {
            List<string> ls = new List<string>
            {
                d.Name
            };

            var parent_d = (from Departament in bd.Departaments
                            where Departament.Id == d.Parentid
                            select Departament).FirstOrDefault();
            if (parent_d != null)
            {
                ls.AddRange(ShowDepartamentByID(bd, parent_d));
            }

            return ls;

        }
        static List<string> EandM(BdContext bd, Departament d, List<string> ls)
        {
            string f = "=";
            ls.Reverse();
            for (int i = 0; i < ls.Count; i++)
            {
                ls[i] = ls[i].Insert(0, f);
                f = f + "=";
            }

            string space = "";
            space = space.PadLeft(ls.Count, ' ');
            var jobtitle = "";
            var manager = "";


            manager = (from Employee in bd.Employees
                       where Employee.Id == d.Managerid
                       select Employee.Fullname).FirstOrDefault();
            var jobid = (from Employee in bd.Employees
                         where Employee.Id == d.Managerid
                         select Employee.Jobtitleid).FirstOrDefault();
            jobtitle = (from Jobtitle in bd.Jobtitles
                        where Jobtitle.Id == jobid
                        select Jobtitle.Name).FirstOrDefault();
            if (manager != null && jobid != null && jobtitle != null)
            {
                ls.Add($"{space}*{manager} ID={d.Managerid} ({jobtitle} ID={jobid})");
            }

            var employee = (from Employee in bd.Employees
                            where Employee.Departament == d.Id
                            select Employee).ToList();
            foreach (Employee emp in employee)
            {
                if (emp.Fullname == manager)
                {
                    continue;
                }
                jobtitle = (from Jobtitle in bd.Jobtitles
                            where Jobtitle.Id == emp.Jobtitleid
                            select Jobtitle.Name).FirstOrDefault();
                if (jobtitle != null)
                {
                    ls.Add($"{space}-{emp.Fullname} ID={emp.Id} ({jobtitle} ID={emp.Jobtitleid})");

                }
            }
            return ls;

        }
    }

}

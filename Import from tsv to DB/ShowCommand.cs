using System;
using System.Collections.Generic;
using System.Linq;
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

                    manager = list_departaments[i].Manager?.Fullname;
                    var jobid = list_departaments[i].Manager?.Jobtitleid;
                    jobtitle = list_departaments[i].Manager?.Jobtitle?.Name;
                    if (manager != null && jobid != null && jobtitle != null)
                    {
                        ls.Add($"{space}*{manager} ID={list_departaments[i].Managerid} ({jobtitle} ID={jobid})");
                    }

                    foreach (Employee emp in list_departaments[i].Employees)
                    {
                        if (emp.Fullname == manager)
                        {
                            continue;
                        }
                        jobtitle = emp.Jobtitle?.Name;
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

            var parent_d = d.Parent;
            while (parent_d != null)
            {
                ls.Insert(0, parent_d.Name);
                parent_d = parent_d.Parent;
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

            manager = d.Manager?.Fullname;
            var jobid = d.Manager?.Jobtitleid;
            jobtitle = d.Manager?.Jobtitle?.Name;
            if (manager != null && jobid != null && jobtitle != null)
            {
                ls.Add($"{space}*{manager} ID={d.Managerid} ({jobtitle} ID={jobid})");
            }

            foreach (Employee emp in d.Employees)
            {
                if (emp.Fullname == manager)
                {
                    continue;
                }
                jobtitle = emp.Jobtitle?.Name;
                if (jobtitle != null)
                {
                    ls.Add($"{space}-{emp.Fullname} ID={emp.Id} ({jobtitle} ID={emp.Jobtitleid})");

                }
            }
            return ls;
        }
    }
}

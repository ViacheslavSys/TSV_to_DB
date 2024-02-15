using System;
using System.IO;
using System.Linq;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class DepartamentImporter
    {
        public void ImportDepartament(string fileName)
        {
            using var bd = new BdContext();

            var lines = File.ReadLines(fileName).Skip(1); // Ленивая загрузка строк из файла

            foreach (var line in lines)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(line) && line != "\t\t\t")
                    {
                        var values = line.Split('\t').Select(v => v.Trim());

                        values = values.Select((v, i) =>
                            i switch
                            {
                                0 => StringHelper.FirstUpper(v, false).Replace("  "," "), // Первая буква в верхний регистр
                                3 => v.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", ""),
                                _ => v
                            });

                        Departament d = new Departament
                        {
                            Name = values.ElementAt(0),
                            Parentid = bd.Departaments.FirstOrDefault(dept => dept.Name == values.ElementAt(1))?.Id,
                            Managerid = bd.Employees.FirstOrDefault(emp => emp.Fullname == values.ElementAt(2))?.Id,
                            Phone = values.ElementAt(3)
                        };


                        if (d.Parentid != null)
                        {
                            Departament departamentUpdate = bd.Departaments.FirstOrDefault(s => s.Name == d.Name && s.Parentid == d.Parentid);
                            UpdateOrCreateDepartaments(d.Parentid, d.Name, d.Managerid, d.Phone, bd, departamentUpdate);
                        }
                        else
                        {
                            Departament departamentUpdate = bd.Departaments.FirstOrDefault(s => s.Name == d.Name);
                            UpdateOrCreateDepartaments(d.Parentid, d.Name, d.Managerid, d.Phone, bd, departamentUpdate);
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

        static void UpdateOrCreateDepartaments(long? parentId, string name, long? managerId, string phone, BdContext bd, Departament departamentUpdate)
        {
            if (departamentUpdate != null)
            {
                departamentUpdate.Parentid = parentId;
                departamentUpdate.Managerid = managerId;
                departamentUpdate.Phone = phone;
                bd.Departaments.Update(departamentUpdate);
            }
            else
            {
                Departament departamentNew = new Departament
                {
                    Name = name,
                    Parentid = parentId,
                    Managerid = managerId,
                    Phone = phone
                };
                bd.Departaments.Add(departamentNew);
            }
        }
    }
}

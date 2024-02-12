using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class DepartamentImporter
    {
        public void ImportDepartament(string fileName)
        {

            using var bd = new BdContext();
            var list = File.ReadAllLines(fileName).Skip(1);
            var departamentsInBD = bd.Departaments.ToList();

            foreach (var item in list)
            {
                try
                {
                    if (item != "\t\t\t")
                    {
                        var value = item.Split('\t');
                        for (int i = 0; i < value.Length; i++)
                        {
                            value[i] = value[i].TrimStart(' ');
                            value[i] = value[i].TrimEnd(' ');
                            value[i] = value[i].Replace("  ", " ");
                        }
                        value[3] = value[3].Replace(" ", "");
                        value[3] = value[3].Replace("(", "");
                        value[3] = value[3].Replace(")", "");
                        value[3] = value[3].Replace("-", "");

                        value[0] = StringHelper.FirstUpper(value[0], false);
                        Departament d = new Departament();
                        d.Name = value[0];


                        d.Managerid = (from Emmployee in bd.Employees
                                       where Emmployee.Fullname == value[2]
                                       select Emmployee.Id).FirstOrDefault();

                        d.Phone = value[3];
                        d.Parentid = (from Departament in bd.Departaments
                                      where Departament.Name == value[1]
                                      select Departament.Id).FirstOrDefault();
                        if (d.Parentid != null)
                        {
                            Departament departament_update = bd.Departaments.FirstOrDefault(s => s.Name == d.Name && s.Parentid == d.Parentid);
                            Update_or_Create_departaments(d.Parentid, d.Name, d.Managerid, d.Phone, bd, departament_update);
                        }
                        else
                        {                           
                            Departament departament_update = bd.Departaments.FirstOrDefault(s => s.Name == d.Name);
                            Update_or_Create_departaments(d.Parentid, d.Name, d.Managerid, d.Phone, bd, departament_update);
                        }
                        bd.SaveChanges();
                    }
            }
                catch { Console.WriteLine("stderror"); }

        }
        }
        static void Update_or_Create_departaments(long? parent_id, string name, long? manager_id, string phone, BdContext bd, Departament departament_update)
        {
            if (departament_update != null)
            {
                departament_update.Name = name;
                departament_update.Parentid = parent_id;
                departament_update.Managerid = manager_id;
                departament_update.Phone = phone;
                bd.Departaments.Update(departament_update);
            }
            else
            {
                Departament departament_new = new Departament
                {
                    Name = name,
                    Parentid = parent_id,
                    Managerid = manager_id,
                    Phone = phone,
                };
                bd.Departaments.Add(departament_new);
            }
        }
    }

}

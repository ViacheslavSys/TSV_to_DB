using Import_from_tsv_to_DB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_from_tsv_to_DB.Working_with_arguments
{
    internal static class ImportCommand
    {
        public static void Execute(string[] args, int index)
        {
            if (index + 2 >= args.Length)
            {
                Console.WriteLine("Missing argument!");
                return;
            }

            string fileName = args[index + 1];
            string typeImport = args[index + 2];

            if (File.Exists(fileName))
            {
                ImportHandler.Import(fileName, typeImport);
                ShowOtherTables();
                ShowCommand.Departments();
            }
            else
            {
                Console.WriteLine("Fail path!");
            }
        }
        static void ShowOtherTables()
        {
            using var bd = new BdContext();

            // получаем объекты из бд и выводим на консоль
            var jobtitles = bd.Jobtitles.OrderBy(s => s.Id);
            Console.WriteLine("\nJob titles:");
            foreach (Jobtitle j in jobtitles)
            {
                Console.WriteLine($"{j.Id}.{j.Name}");
            }

            var employees = bd.Employees.OrderBy(s=>s.Id).ToList();
            Console.WriteLine("\nEmployees");
            foreach (Employee e in employees)
            {
                var departament = "";
                var jobtitle = "";

                departament = (from Departament in bd.Departaments
                               where Departament.Id == e.Departament
                               select Departament.Name).FirstOrDefault();


                jobtitle = (from Jobtitle in bd.Jobtitles
                            where Jobtitle.Id == e.Jobtitleid
                            select Jobtitle.Name).FirstOrDefault();
            
               

                Console.WriteLine($"{e.Id}.{departament}(id = {e.Departament}) {e.Fullname} {e.Login} {e.Password} {jobtitle}(id = {e.Jobtitleid})");
            }
        }

    }
}

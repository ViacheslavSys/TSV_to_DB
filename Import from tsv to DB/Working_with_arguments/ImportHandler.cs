using Import_from_tsv_to_DB.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_from_tsv_to_DB.Working_with_arguments
{
    internal static class ImportHandler
    {
        public static void Import(string fileName, string typeImport)
        {
            switch (typeImport)
            {
                case "-d":
                    new DepartamentImporter().ImportDepartament(fileName);
                    break;
                case "-e":
                    new EmployeeImporter().ImportEmployee(fileName);
                    break;
                case "-j":
                    new JobTitleImporter().ImportJobTitle(fileName);
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }
    }
}

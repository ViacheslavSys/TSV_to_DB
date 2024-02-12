using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_from_tsv_to_DB.Working_with_arguments
{
    internal static class HelpCommand
    {
        public static void ShowHelp()
        {
            Console.WriteLine("------Commands------" +
                "\n-help                              \tShow help" +
                "\n-i,  --import <path> <type_import> \tImport a file into a database" +
                "\n-s,  --show                        \tOutput the current data structure " +
                "\n-si, --show_id                     \tOutput only the chain of parent divisions (without employees) before it, the division itself and its employees" +
                "\n---Types of import---" +
                "\n-d\tDepartament\n-e\tEmployee\n-j\tJob_title");
        }
    }
}

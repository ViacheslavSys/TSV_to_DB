using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_from_tsv_to_DB.Working_with_arguments
{
    internal static class ArgumentHandler
    {
        public static void HandleArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-help")
                {
                    HelpCommand.ShowHelp();
                    break;
                }
                else if (args[i] == "-i" || args[i] == "--import")
                {
                    ImportCommand.Execute(args, i);
                }
                else if (args[i] == "-s" || args[i] == "--show")
                {
                    ShowCommand.Departments();
                }
                else if (args[i] == "-si" || args[i] == "--show_id")
                {
                    long id = Convert.ToInt64(args[i + 1]);
                    ShowCommand.DepartmentByID(id);
                }
            }
        }
    }
}

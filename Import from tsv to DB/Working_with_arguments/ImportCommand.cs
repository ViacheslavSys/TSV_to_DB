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
            }
            else
            {
                Console.WriteLine("Fail path!");
            }
        }
    }
}

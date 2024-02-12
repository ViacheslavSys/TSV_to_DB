using Import_from_tsv_to_DB.Working_with_arguments;
using System;
using System.IO;
using System.Linq;


namespace TSV_to_DB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ArgumentHandler.HandleArguments(args);
        }        
    }
}

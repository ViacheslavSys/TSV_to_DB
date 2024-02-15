using System;
using System.IO;
using System.Linq;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class JobTitleImporter
    {
        public void ImportJobTitle(string fileName)
        {
            using var bd = new BdContext();

            var lines = File.ReadLines(fileName).Skip(1);

            foreach (var line in lines)
            {
                try
                {
                    var temp = line.Replace("  ", " ").Trim(); 
                    if (!string.IsNullOrWhiteSpace(temp)) 
                    {
                        var jobTitleUpdate = bd.Jobtitles.FirstOrDefault(x => x.Name == temp);
                        if (jobTitleUpdate != null)
                        {
                            jobTitleUpdate.Name = temp;
                            bd.Jobtitles.Update(jobTitleUpdate);
                        }
                        else
                        {
                            var jobTitleNew = new Jobtitle
                            {
                                Name = temp
                            };
                            bd.Jobtitles.Add(jobTitleNew);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Import_from_tsv_to_DB.Models;

namespace Import_from_tsv_to_DB.Import
{
    internal class JobTitleImporter
    {
        public void ImportJobTitle(string fileName)
        {
            using var bd = new BdContext();

            var list = File.ReadAllLines(fileName).Skip(1);

            foreach (var line in list)
            {
                try
                {
                    var temp = line.Replace("  ", " ");
                    temp = temp.TrimStart(' ');
                    temp = temp.TrimEnd(' ');
                    if (temp.Length > 0)
                    {
                        Jobtitle jobtitle_update = bd.Jobtitles.FirstOrDefault(x => x.Name == temp);
                        if (jobtitle_update != null)
                        {
                            jobtitle_update.Name = temp;
                            bd.Jobtitles.Update(jobtitle_update);
                        }
                        else
                        {
                            Jobtitle jobtitle_new = new Jobtitle
                            {
                                Name = temp
                            };
                            bd.Jobtitles.Add(jobtitle_new);
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

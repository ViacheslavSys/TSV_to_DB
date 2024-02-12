using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_from_tsv_to_DB.Import
{
    public static class StringHelper
    {
        public static string FirstUpper(string str, bool eachWord = false)
        {
            if (eachWord)
            {
                string[] words = str.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 1)
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                    else
                        words[i] = words[i].ToUpper();
                }
                return string.Join(" ", words);
            }
            else
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = char.ToUpper(str[0]) + str.Substring(1).ToLower();
                }
                return str;
            }
        }
    }
}

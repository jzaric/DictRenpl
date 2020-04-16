using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DictRename
{
    class Program
    {
        /// <summary>
        /// Replaces strings in files based on input dictionary
        /// </summary>
        /// <param name="dictFile">.json dictionary file</param>
        /// <param name="extensionFilter">In which extensions to execute the replace</param>
        /// <returns></returns>
        static int Main(FileInfo dictFile, string extensionFilter = "*.*")
        {
            var dictFileJson = File.ReadAllText(dictFile.FullName);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictFileJson);
            Console.WriteLine($"{dict.Count} pairs in dict.");
            var files = Directory.EnumerateFiles(Environment.CurrentDirectory, extensionFilter).ToList();
            Console.WriteLine($"{files.Count} files found.");
            foreach (var f in files)
            {
                var fileInfo = new FileInfo(f);
                Console.WriteLine($"Processing {f}...");
                var contents = File.ReadAllText(f);
                foreach (var kvp in dict)
                {
                    int count = Regex.Matches(contents, Regex.Escape(kvp.Key)).Count;
                    contents = contents.Replace(kvp.Key, kvp.Value);
                    Console.WriteLine($"Replaced {count} instances of {kvp.Key} with {kvp.Value}.");
                }
                File.WriteAllText(f, contents);
            }
            return 0;
        }
    }
}

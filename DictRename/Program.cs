using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DictRename
{
    class Program
    {
        /// <summary>
        /// Renames files in dir based on input dictionary
        /// </summary>
        /// <param name="dictFile">.json dictionary file</param>
        /// <param name="extensionFilter">Extension filter</param>
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
                var newFileName = fileInfo.Name;
                foreach (var kvp in dict)
                {
                    newFileName = newFileName.Replace(kvp.Key, kvp.Value);
                }
                if (newFileName != fileInfo.Name)
                {
                    var newFullName = Path.Combine(fileInfo.DirectoryName, newFileName);
                    File.Move(fileInfo.FullName, newFullName);
                    Console.WriteLine($"-> {newFullName}");
                }
                else
                {
                    Console.WriteLine($"No matches.");
                }
            }
            return 0;
        }
    }
}

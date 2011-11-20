using System;
using System.Collections.Generic;
using System.IO;

namespace DistributedDatabase.TransactionManager.InputUtilities
{
    /// <summary>
    /// Loads a file from the disk.
    /// </summary>
    public class FileReader
    {
        private string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReader"/> class.
        /// </summary>
        /// <param name="fileToRead">The file to read.</param>
        public FileReader(string fileToRead)
        {
            _filePath = fileToRead;
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <returns></returns>
        public List<String> ReadFile()
        {
            List<String> lines = new List<string>();

            if (System.IO.File.Exists(_filePath))
            {
                string[] fileLines = System.IO.File.ReadAllLines(_filePath);

               foreach (String tempString in fileLines)
               {
                   lines.Add(tempString);
               }
            }
            else
                throw new FileNotFoundException();

            return lines;
        }
    }
}

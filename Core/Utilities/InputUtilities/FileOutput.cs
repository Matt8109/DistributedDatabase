using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedDatabase.Core.Utilities.InputUtilities
{
   public static class FileOutput
    {
        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
       public static void WriteFile (String fileName, List<String> content)
       {
            try
            {
           System.IO.File.WriteAllLines(fileName + ".out.txt", content);
            }
            catch (Exception)
            {
                
              Console.WriteLine("Error writing to file.");
            }

       }
    }
}

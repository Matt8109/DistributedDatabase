using System;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Execution;

namespace DistributedDatabase.Core.Utilities.InputUtilities
{
    public static class InputParser
    {
        /// <summary>
        /// Reads the file input and converts it into a list of execution plans.
        /// </summary>
        /// <param name="fileInput">The file input.</param>
        /// <param name="errorList">The error list.</param>
        /// <returns></returns>
        public static List<ExecutionEntity> ReadInput(List<string> fileInput, ref List<String> errorList )
        {
            var entities = new List<ExecutionEntity>();

            foreach (String currentLine in fileInput)
            {
                var lineElements = BreakLine(currentLine);
                
                
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Breaks up multiple entries on a line if needed, i.e. if they
        /// are separated by a semicolon
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static List<String> BreakLine(string line)
        {
            //split based on the semicolon
            string[] temp = line.Split(';');
           
            for (int i=0; i<temp.Length; i++)
            {
                temp[i] = temp[i].Trim();
            }

            return temp.ToList();
        }

        public static BaseAction StringToAction(String lineElement, ref List<String>  errorList)
        {

            var command = lineElement.Split('(').FirstOrDefault();

            if (String.IsNullOrEmpty(command))
            {
                errorList.Add("Invalid Command Format: " + lineElement);
                return null;
            }

            command = command.ToLower();

            if (command.Equals("begin"))
            {
                
            }

            throw new NotImplementedException();
        }
    }
}

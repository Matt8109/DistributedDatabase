using System;
using System.Collections.Generic;
using System.Linq;
using DistributedDatabase.Core.Entities;
using DistributedDatabase.Core.Entities.Actions;
using DistributedDatabase.Core.Entities.Execution;
using DistributedDatabase.Core.Entities.Sites;
using DistributedDatabase.Core.Entities.Transactions;

namespace DistributedDatabase.Core.Utilities.InputUtilities
{
    public static class InputParser
    {
        /// <summary>
        /// Reads the file input and converts it into a list of execution plans.
        /// </summary>
        /// <param name="fileInput">The file input.</param>
        /// <param name="transactionList">The transaction list.</param>
        /// <param name="siteList">The site list.</param>
        /// <param name="systemClock">The system clock.</param>
        /// <param name="errorList">The error list.</param>
        /// <returns></returns>
        public static List<ExecutionEntity> ReadInput(List<string> fileInput, TransactionList transactionList,
                                                      SiteList siteList, SystemClock systemClock,
                                                      ref List<String> errorList)
        {
            var entities = new List<ExecutionEntity>();

            foreach (String currentLine in fileInput)
            {
                List<string> lineElements = BreakLine(currentLine);
                var tempExecutionEntiy = new ExecutionEntity();

                foreach (String tempElement in lineElements)
                {
                    BaseAction result = StringToAction(tempElement, transactionList, siteList, systemClock,
                                                       ref errorList);

                    if (result != null)
                        tempExecutionEntiy.AddAction(result);
                }

                entities.Add(tempExecutionEntiy);
            }

            return entities;
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

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i].Trim();
            }

            return temp.ToList();
        }

        public static string[] TrimStringList(string[] list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                list[i] = list[i].Trim();
            }

            return list;
        }

        public static BaseAction StringToAction(String lineElement, TransactionList transactionList, SiteList siteList,
                                                SystemClock systemClock, ref List<String> errorList)
        {
            lineElement = lineElement.Trim();
            string command = lineElement.Split('(').FirstOrDefault();

            if (String.IsNullOrEmpty(command))
            {
                errorList.Add("Invalid Command Format: " + lineElement);
                return null;
            }

            command = command.ToLower();

            if (command.Equals("begin") || command.Equals("beginro"))
                return new BeginTransaction(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("end"))
                return new EndTransaction(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("read"))
                return new Read(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("write"))
                return new Write(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("fail"))
                return new Fail(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("recover"))
                return new Fail(lineElement, transactionList, siteList, systemClock);

            if (command.Equals("dump"))
                return new Dump(lineElement, transactionList, siteList, systemClock);

            errorList.Add("Couldn't interpret: " + lineElement);
            return null;
        }
    }
}
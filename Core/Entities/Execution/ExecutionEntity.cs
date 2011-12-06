using System.Collections.Generic;
using DistributedDatabase.Core.Entities.Actions;

namespace DistributedDatabase.Core.Entities.Execution
{
    /// <summary>
    /// Holds the list of actions that needs to be executed
    /// in a single execution plan tick.
    /// </summary>
    public class ExecutionEntity
    {
        public ExecutionEntity()
        {
            Actions = new List<BaseAction>();
        }

        public ExecutionEntity(BaseAction action)
        {
            Actions = new List<BaseAction>();
            Actions.Add(action);
        }

        public List<BaseAction> Actions { get; set; }

        public void AddAction(BaseAction action)
        {
            Actions.Add(action);
        }
    }
}
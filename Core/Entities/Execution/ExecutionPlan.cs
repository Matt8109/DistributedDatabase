//using System.Collections.Generic;

//namespace DistributedDatabase.Core.Entities.Execution
//{
//    /// <summary>
//    /// Holds the entire execution plan.
//    /// </summary>
//    public class ExecutionPlan
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ExecutionPlan"/> class.
//        /// </summary>
//        /// <param name="executionPlan">The execution plan.</param>
//        public ExecutionPlan(List<ExecutionEntity> executionPlan)
//        {
//            Plan = new List<ExecutionEntity>();

//            executionPlan.ForEach(x => Plan.Add(x));
//        }

//        /// <summary>
//        /// Gets or sets the  execution plan
//        /// </summary>
//        /// <value>
//        /// The plan.
//        /// </value>
//        public List<ExecutionEntity> Plan { get; set; }

//        /// <summary>
//        /// Adds a single execution entity to the plan.
//        /// </summary>
//        /// <param name="entity">The entity.</param>
//        public void AddExecutionEntity(ExecutionEntity entity)
//        {
//            Plan.Add(entity);
//        }
//    }
//}
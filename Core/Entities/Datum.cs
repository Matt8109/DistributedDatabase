using System;

namespace DistributedDatabase.DataLockManager.Entities
{
	public class Datum
	{
		/// <summary>
		/// Gets or sets the id of the variable.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		String Id {get;set;}
		
		/// <summary>
		/// Gets or sets the committed value of the variable.
		/// </summary>
		/// <value>
		/// The committed value.
		/// </value>
		Object CommittedValue {get;set;}
		
		/// <summary>
		/// Gets or sets the current value of the variable. Will be empty if no changes
		/// have been made.
		/// </summary>
		/// <value>
		/// The current value.
		/// </value>
		Object CurrentValue {get;set;}
		
		/// <summary>
		/// The id of the transaction that last wrote successfully to the variable.
		/// </summary>
		/// <value>
		/// The last writer.
		/// </value>
		int LastWriter {get;set;}
		
		public Datum ()
		{
		}
	}
}


using System;
using DistributedDatabase.TransactionManager.Entities;

namespace DataLockManager
{
	public interface IObjectGraph
	{
		/// <summary>
		/// Variables the exists at this site..
		/// </summary>
		/// <returns>
		/// The exists.
		/// </returns>
		/// <param name='id'>
		/// If set to <c>true</c> identifier.
		/// </param>
		bool VariableExists(string id);
		
		/// <summary>
		/// Determines whether the variable of the given Id is currently uncommitted.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is uncommitted the specified id; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='id'>
		/// If set to <c>true</c> identifier.
		/// </param>
		bool IsUncommited(string id);
		
		/// <summary>
		/// Gets the variable by a given id. This can be used to set or otherwise
		/// changed variable.
		/// </summary>
		/// <returns>
		/// The variable.
		/// </returns>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		Variable GetVariable(string id);
	}
}


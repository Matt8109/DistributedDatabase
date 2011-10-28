using System;
using System.Collections.Generic;
using DistributedDatabase.TransactionManager.Entities;

namespace DataLockManager
{
	public interface ILockManager
	{
		/// <summary>
		/// Determines whether this the variable with the given id is locked or not.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is locked the specified Id; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='Id'>
		/// If set to <c>true</c> identifier.
		/// </param>
		bool IsLocked(string Id);
		
		/// <summary>
		/// Attempts to get the lock for a variable. You can optionally specify
		/// if the lock is not a read lock. 
		/// </summary>
		/// <returns>
		/// The lock.
		/// </returns>
		/// <param name='Id'>
		/// If set to <c>true</c> identifier.
		/// </param>
		/// <para name ='seeker'>
		/// The transaction that seeks the lock
		/// </param>
		/// <param name='isReadLock'>
		/// If set to <c>true</c> is read lock.
		/// </param>
		bool GetLock(string Id, Transaction seeker, bool isReadLock = false);
		
		/// <summary>
		/// Realeases a lock.
		/// </summary>
		/// <returns>
		/// The lock.
		/// </returns>
		/// <param name='id'>
		/// If set to <c>true</c> identifier.
		/// </param>
		bool RealeaseLock(string id);
		
		/// <summary>
		/// Returns a reference to the transaction that owns the lock on a variable.
		/// This will be null if a variable is currently not held on the lock.
		/// </summary>
		/// <returns>
		/// The lock.
		/// </returns>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		Transaction OwnsLock(string id);
	}
}


using System.Collections.Generic;
using DistributedDatabase.TransactionManager.Entities;

namespace DistributedDatabase.TransactionManager.Interfaces
{
	public interface ITransactionGraph
	{
		/// <summary>
		/// Adds a new transaction to the transaction graph.
		/// </summary>
		/// <param name='tempTransaction'>
		/// Temp transaction.
		/// </param>
		void AddTransaction(Transaction tempTransaction);
		
		/// <summary>
		/// Returns the status of a transaction based on the id of the transaction.
		/// This allows for the querying functionality.
		/// </summary>
		/// <returns>
		/// The transaction status.
		/// </returns>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		Transaction GetTransactionStatus(int id);
		
		/// <summary>
		/// Gets all transactions currently in the transaction graph. Optionally
		/// can specify whether the transaction graph returns a list of running
		/// transactions only.
		/// </summary>
		/// <returns>
		/// The all transactions.
		/// </returns>
		/// <param name='runningOnly'>
		/// Running only.
		/// </param>
		List<Transaction> GetAllTransactions(bool runningOnly = false);
		
		/// <summary>
		/// Returns a list of blocked transactions. We may want to check them after a tick
		/// to see if they have become unblocked.
		/// </summary>
		/// <returns>
		/// The blocked transactions.
		/// </returns>
		List<Transaction> GetBlockedTransactions();
		
		/// <summary>
		/// Abortsa given transaction by id.
		/// </summary>
		/// <param name='id'>
		/// Identifier.
		/// </param>
		void AbortTransaction(int id);

		/// <summary>
		/// Returns the status of a transaction based on the results of the wait-die protocol.
		/// </summary>
		/// <returns>
		/// The wait die status.
		/// </returns>
		/// <param name='transaction'>
		/// Transaction that was looking for the lock.
		/// </param>
		/// <param name='lockHolder'>
		/// The transaction that holds the lock.
		/// </param>
		TransactionStatus PickWaitDieStatus(Transaction transaction, Transaction lockHolder);
	}
}
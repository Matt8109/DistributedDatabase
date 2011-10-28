using System;
using System.IO.Pipes;

namespace DataLockManager
{
	public interface INetworking
	{
		/// <summary>
		/// Opens a given pipe while waiting for a message.
		/// </summary>
		/// <returns>
		/// The pipe.
		/// </returns>
		/// <param name='pipeId'>
		/// The name of the pipe.
		/// </param>
		bool OpenPipe(string Name);
		
		/// <summary>
		/// Closes the  pipe. This will not throw an exception if the pipe is already closed.
		/// Or was never open.
		/// </summary>
		/// <param name='pipeId'>
		/// pipe identifier.
		/// </param>
        void ClosePipe();
		
		/// <summary>
		/// Lets the program block its thread until a message is received from 
		/// the transaction manager.
		/// </summary>
		/// <returns>
		/// The message.
		/// </returns>
		/// <param name='pipeId'>
		/// pipe identifier.
		/// </param>
        string AwaitMessage();
	}
}


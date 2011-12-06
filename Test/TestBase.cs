using System.Collections.Generic;
using DistributedDatabase.Core.Entities.StateHolder;
using Rhino.Mocks;

namespace DistributedDatabase.Test
{
    /// <summary>
    ///   Unit test base class
    /// </summary>
    public abstract class TestBase
    {
        public TestBase()
        {
            State.output=new List<string>();
        }
        /// <summary>
        ///   Create a mock
        /// </summary>
        /// <typeparam name = "T">Type to be mocked</typeparam>
        /// <param name = "argumentsForConstructor">Constructor arguments</param>
        /// <returns>TMessage</returns>
        protected static T M<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateMock<T>(argumentsForConstructor);
        }

        /// <summary>
        ///   Create a stub
        /// </summary>
        /// <typeparam name = "T">Type to be stubbed</typeparam>
        /// <param name = "argumentsForConstructor">Constructor arguments</param>
        /// <returns>TMessage</returns>
        protected static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }
    }
}
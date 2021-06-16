using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FireSharp.Tests.Core.Commom
{
    public static class AssertExtensions
    {
        public static async Task ThrowsAsync<TException>(Func<Task> func) where TException : Exception
        {
            Type expected = typeof(TException);
            Type actual = null;

            try
            {
                await func();
            }
            catch (TException exception)
            {
                actual = exception.GetType();
            }

            Assert.AreEqual(expected, actual);
        }
   
    }
}

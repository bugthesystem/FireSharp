using System.Collections.Generic;

namespace FireSharp
{
    /// <summary>
    ///     This class contains .NET representations of Firebase Server values.
    /// </summary>
    public static class ServerValues
    {
        /// <summary>
        ///     The time since UNIX epoch, in milliseconds.
        /// </summary>
        public static IDictionary<string, string> Timestamp => new Dictionary<string, string>
        {
            [".sv"] = "timestamp"
        };
    }
}

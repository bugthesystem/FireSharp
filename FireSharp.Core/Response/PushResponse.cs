using System;
using System.Net;
using System.Runtime.Serialization;

namespace FireSharp.Core.Response
{
    public class PushResponse : FirebaseResponse
    {
        public PushResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }

        public PushResult Result => ResultAs<PushResult>();
    }

    [DataContract]
    public class PushResult
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [Obsolete("Use the Name property instead")]
        public string name => Name;
    }
}

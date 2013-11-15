namespace FireSharp.Serialization.ServiceStack {
    using RestSharp.Serializers;
    /// <summary>
    /// ServiceSatck JSON serializer for request bodies
    /// Doesn't currently use the SerializeAs attribute, defers to ServiceStack's attributes that ServiceStack.Text supports DataContract,DataMember attributes
    /// </summary>
    public class ServiceStackJsonSerializer : ISerializer {

        /// <summary>
        /// Default serializer
        /// </summary>
        public ServiceStackJsonSerializer() {
            ContentType = "application/json";

        }

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj) {
            return global::ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// Unused for JSON Serialization
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Content type for serialized content
        /// </summary>
        public string ContentType { get; set; }
    }
}
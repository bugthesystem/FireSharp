using FireSharp.Interfaces;
using System;

namespace FireSharp.Config
{
    public class FirebaseConfig : IFirebaseConfig
    {
        private string _basePath;
        private ISerializer _serializer;

        public FirebaseConfig()
        {
            Serializer = new JsonNetSerializer();
        }

        public string BasePath
        {
            get { return _basePath.EndsWith("/") ? _basePath : $"{_basePath}/"; }
            set { _basePath = value; }
        }

        public string Host { get; set; }

        public string AuthSecret { get; set; }

        /// <summary>
        ///     Gets or sets the request timeout.
        /// </summary>
        /// <value>
        ///     The request timeout.
        /// </value>
        public TimeSpan? RequestTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the serializer instance.
        /// </summary>
        /// <value>
        ///     The currently used serializer.
        /// </value>
        /// <exception cref="System.ArgumentNullException">If an attempt to set a <code>null</code> instance is attempted</exception>
        public ISerializer Serializer
        {
            get { return _serializer; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _serializer = value;
            }
        }
    }
}

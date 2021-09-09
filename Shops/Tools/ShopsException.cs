using System;
using System.Runtime.Serialization;

namespace Shops.Tools
{
    public class ShopsException : Exception
    {
        public ShopsException()
        {
        }

        public ShopsException(string message)
            : base(message)
        {
        }

        public ShopsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ShopsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
using System;

namespace WilliamsonFamily.Library.Exceptions
{
    [Serializable]
    public class InjectablePropertyNullException : ApplicationException
    {
        public InjectablePropertyNullException() { }
        public InjectablePropertyNullException(string propertyName) : base(string.Format("The {0} property has not been initialized. Please configure injectables", propertyName)) { }
        public InjectablePropertyNullException(string message, Exception inner) : base(message, inner) { }
        protected InjectablePropertyNullException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
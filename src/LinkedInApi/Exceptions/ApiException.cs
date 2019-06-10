using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.Exceptions
{   
    [Serializable]
    public class ApiException : Exception
    {       
        public ApiException(ExceptionModel exception) : base(exception.ToString()) { }
        public ApiException(ExceptionModel exception, Exception inner) : base(exception.ToString(), inner) { }
        protected ApiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

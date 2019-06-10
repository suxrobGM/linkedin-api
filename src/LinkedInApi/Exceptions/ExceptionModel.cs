using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedIn.Api.Exceptions
{
    public partial class ExceptionModel
    {        
        public string Message { get; set; }       
        public int ServiceErrorCode { get; set; }        
        public int Status { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Exception message: {Message}");
            builder.AppendLine($"Service error code: {ServiceErrorCode}");
            builder.AppendLine($"Status: {Status}");
            return builder.ToString();
        }
    }

    public partial class ExceptionModel
    {
        public static ExceptionModel FromJson(string json) => JsonConvert.DeserializeObject<ExceptionModel>(json, CustomConverter.Settings);
    }

    public static class ExceptionModelSerialize
    {
        public static string ToJson(this ExceptionModel self) => JsonConvert.SerializeObject(self, CustomConverter.Settings);
    }
}

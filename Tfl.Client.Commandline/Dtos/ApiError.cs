using System;
using System.Net;

namespace Tfl.Client.Commandline.Dtos
{
    public class ApiError
    {
        public DateTime TimestampUtc { get; set; }
        public string ExceptionType { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public string RelativeUri { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"RelativeUri:{RelativeUri}{Environment.NewLine}HttpStatusCode:{HttpStatusCode}{Environment.NewLine}HttpStatus:{HttpStatus}{Environment.NewLine}Message:{Message}{Environment.NewLine}TimestampUtc:{TimestampUtc}{Environment.NewLine}ExceptionType:{ExceptionType}{Environment.NewLine}";
        }
    }
}

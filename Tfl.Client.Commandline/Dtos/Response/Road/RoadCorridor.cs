using System;

namespace Tfl.Client.Commandline.Dtos.Response.Road
{
    public class RoadCorridor
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string StatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; }
        public string Bounds { get; set; }
        public string Envelope { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}{Environment.NewLine}DisplayName:{DisplayName}{Environment.NewLine}StatusSeverity:{StatusSeverity}{Environment.NewLine}StatusSeverityDescription:{StatusSeverityDescription}{Environment.NewLine}Bounds:{Bounds}{Environment.NewLine}Envelope:{Envelope}{Environment.NewLine}Url:{Url}{Environment.NewLine}";
        }
    }
}
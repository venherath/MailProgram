using System;

namespace MailProgram
{
    public class LogLine(string message, LogLineTypeEnum type, object tag = null)
    {
        public string Message { get; set; } = message;
        public LogLineTypeEnum Type { get; set; } = type;
        public DateTime Date { get; private set; } = DateTime.Now;
        public object Tag { get; set; } = tag;
    }
}

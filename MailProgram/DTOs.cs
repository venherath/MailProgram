using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace MailProgram
{
    public class SMTPSettingsDTO
    {
        public string SmtpServerUrl { get; set; }
        public string SmtpServerName => SmtpServerUrl.Split(':').FirstOrDefault();
        public string SmtpServerPort => SmtpServerUrl.Split(':').LastOrDefault();
        public bool SmtpServerEnableTSL { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
        public string SignatureHTML { get; set; }
        public string SignatureImageName { get; set; }
        public byte[] SignatureImageBytes { get; set; }
        public string DebugBcc { get; set; }
        public string FromEMail { get; set; }
    }

    public class MailSignature
    {
        public string SenderName { get; set; }
        public string SenderJobTitle { get; set; }
        public string SenderDepartment { get; set; }
        public string SignatureHTML { get; set; }
        public string SignatureLogoFilePath { get; set; }
    }

    //public class LogService
    //{
    //    public bool IsEnabled { get; set; }
    //    public string ApplicationProductName { get; set; }
    //    public string MachineName { get; set; }
    //    public string UserId { get; set; }
    //    public DateTime Date { get; set; }

    //    public LogService()
    //    {
    //        Date = DateTime.Now;
    //        MachineName = Environment.MachineName;
    //    }

    //    public void Dump(object collection, string name)
    //    {
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Dump(collection, name);
    //    }

    //    public void Debug(string message)
    //    {
    //        if (!IsEnabled) return;
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Log(new LogMessage(message, LogLevelEnum.Debug, UserId));
    //    }

    //    public void Info(string message)
    //    {
    //        if (!IsEnabled) return;
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Log(new LogMessage(message, LogLevelEnum.Info, UserId));
    //    }

    //    public void Error(string message)
    //    {
    //        if (!IsEnabled) return;
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Log(new LogMessage(message, LogLevelEnum.Error, UserId));
    //    }

    //    public void Warning(string message)
    //    {
    //        if (!IsEnabled) return;
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Log(new LogMessage(message, LogLevelEnum.Warning, UserId));
    //    }

    //    public void Fatal(string message)
    //    {
    //        if (!IsEnabled) return;
    //        var lw = new LogWritter { ApplicationName = ApplicationProductName };
    //        lw.Log(new LogMessage(message, LogLevelEnum.Fatal, UserId));
    //    }
    //}

    //public enum LogLevelEnum
    //{
    //    Debug,
    //    Info,
    //    Warning,
    //    Error,
    //    Fatal
    //}

    //// These would be in separate files in practice, but included here for completeness
    //public class LogWritter
    //{
    //    public string ApplicationName { get; set; }
    //    public void Dump(object collection, string name) { }
    //    public void Log(LogMessage message) { }
    //}

    //public class LogMessage
    //{
    //    public LogMessage(string message, LogLevelEnum level, string userId) { }
    //}
}
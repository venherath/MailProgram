using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace MailProgram
{
    public class LoggerService
    {
        public EventHandler<LogLine> LogChanged;
        public bool EnableLog { get; set; } = false;
        public List<LogLine> LogLines { get; set; } = [];
        public long ElapsedMilliseconds => _Clock.ElapsedMilliseconds;// { get; set; }
        public int ErrorCount => LogLines.Count(prop => prop.Type == LogLineTypeEnum.Error);
        public int WarningCount => LogLines.Count(prop => prop.Type == LogLineTypeEnum.Warning);

        private readonly Stopwatch _Clock = new();

        public void ClockStart() => _Clock.Restart();
        public void ClockStop() => _Clock.Stop();

        private static JsonSerializerOptions OptionsWriteIntended => new() { WriteIndented = true };

        public string GetLogContent()
        {
            return JsonSerializer.Serialize(LogLines, OptionsWriteIntended);
        }

        public LoggerService()
        {
            _Clock.Start();
        }

        private void RaiseLogChangedEvent(LogLine line)
        {
            LogChanged?.Invoke(this, line);
        }

        public LoggerService Info(string message, ConsoleColor defaultColor = ConsoleColor.DarkGreen, object tag = null)
        {
            SetColor(defaultColor);
            Console.WriteLine($"INFO  - {message}");
            ResetColor();
            var line = new LogLine(message, LogLineTypeEnum.Info, tag);
            LogLines.Add(line);
            RaiseLogChangedEvent(line);
            return this;
        }

        public LoggerService Warn(string message, ConsoleColor defaultColor = ConsoleColor.Yellow, object tag = null)
        {
            SetColor(defaultColor);
            Console.WriteLine($"WARN! - {message}");
            ResetColor();
            var line = new LogLine(message, LogLineTypeEnum.Warning, tag);
            LogLines.Add(line);
            RaiseLogChangedEvent(line);
            return this;
        }

        public LoggerService Debug(string message, ConsoleColor defaultColor = ConsoleColor.Magenta, object tag = null)
        {
            SetColor(defaultColor);
            if (EnableLog) Console.WriteLine($"DEBUG - {message}");
            ResetColor();
            var line = new LogLine(message, LogLineTypeEnum.Debug, tag);
            LogLines.Add(line);
            RaiseLogChangedEvent(line);
            return this;
        }

        public LoggerService Error(string message, ConsoleColor defaultColor = ConsoleColor.Red, object tag = null)
        {
            SetColor(defaultColor);
            Console.WriteLine($"ERROR - {message}");
            ResetColor();
            var line = new LogLine(message, LogLineTypeEnum.Error, tag);
            LogLines.Add(line);
            RaiseLogChangedEvent(line);
            return this;
        }

        public void SetColor(ConsoleColor defaultColor)
        {
            try
            {
                Console.ForegroundColor = defaultColor;
            }
            catch (Exception)
            {

            }
        }

        public void ResetColor()
        {
            try
            {
                Console.ResetColor();
            }
            catch (Exception)
            {

            }
        }

        public void PrintEmpty()
        {
            Console.WriteLine(string.Empty);
        }

        public void PrintLine(char c = '*', ConsoleColor color = ConsoleColor.Gray, int? charnum = null)
        {
            int max = 80;
            try
            {
                max = Console.WindowWidth - 10;
            }
            catch (Exception)
            {

            }
            Info(new string(c, charnum ?? max), color);
        }
        
        public void SayHello()
        {
            Console.WriteLine($"> HELLO! { DateTime.Now}");
        }

        public void PrintFatalError(Exception ex, string connectionString)
        {
            Error($"FATAL - {ex.Message}\n{ex.InnerException?.Message}");
            Error($"Connection: {connectionString}");
        }

        public void PrintStart(ConsoleColor color = ConsoleColor.Gray)
        {
            var lines = new List<string>();
            var assembly = Assembly.GetEntryAssembly();
            lines.Add($"** STARTED PROCESS AT: {DateTime.Now}");
            lines.Add($"** {assembly.GetName().Name.ToUpper()} " +
                $"({assembly.GetName().Version})");
            lines.Add($"** EXECUTED BY: {Environment.UserDomainName}\\{Environment.UserName} @ {Environment.MachineName}");
            PrintLines(color, lines);
        }

        public void PrintSummary(long elapsedMiliseconds = 0, ConsoleColor color = ConsoleColor.Gray)
        {
            if (ErrorCount > 0) color = ConsoleColor.Red;
            var lines = new List<string>();
            var assembly = Assembly.GetEntryAssembly();
            lines.Add($"** FINISHED PROCESS AT: {DateTime.Now}");
            lines.Add($"** {assembly.GetName().Name.ToUpper()} " +
                $"({assembly.GetName().Version})");
            if (elapsedMiliseconds > 0)
                lines.Add($"** TOTAL : {(decimal)elapsedMiliseconds/1000:n2}s.");
            if (ErrorCount > 0) 
                lines.Add($"** ERRORS: {ErrorCount}");
            PrintLines(color, lines);
        }

        private void PrintLines(ConsoleColor color, List<string> lines)
        {
            PrintLine(color: color);
            PrintLine(color: color, charnum: 2);
            foreach (var line in lines)
                Info(line, color);
            PrintLine(color: color, charnum: 2);
            PrintLine(color: color);
        }

        public void PrintEmailSummary()
        {
            PrintLine('=', ConsoleColor.Cyan);
            Info("EMAIL SENDING SUMMARY", ConsoleColor.Cyan);
            PrintLine('=', ConsoleColor.Cyan);

            Info($"Total operations: {LogLines.Count}");
            Info($"Errors: {ErrorCount}",
                ErrorCount > 0 ? ConsoleColor.Red : ConsoleColor.Green);
            Info($"Warnings: {WarningCount}",
                WarningCount > 0 ? ConsoleColor.Yellow : ConsoleColor.Green);
            Info($"Elapsed time: {ElapsedMilliseconds}ms");

            PrintLine('=', ConsoleColor.Cyan);

            // Optional: Display full log content as JSON
            if (EnableLog)
            {
                Info("Full log available via GetLogContent() method");
            }
        }
    }
}

using MailKit.Net.Smtp;
using MimeKit;
using System.Diagnostics;
using System.Security.Principal;

namespace MailProgram
{
    // Our main email service that handles sending emails with different behaviors for testing vs production
    public class SMTPService
    {
        public LoggerService LogService { get; private set; }
        public SmtpClient MailServer { get; private set; }
        public SMTPSettingsDTO SMTPSettings { get; private set; }
        public string SenderDisplayName { get; set; }

        // Set up the email service with the correct configuration
        public SMTPService(SMTPSettingsDTO smtpSettings, string senderDisplayName = null)
        {
            // If no display name provided, use the SMTP username as default
            if (senderDisplayName == null) senderDisplayName = smtpSettings.SmtpUser;
            SMTPSettings = smtpSettings;
            SenderDisplayName = senderDisplayName;

            // Initialize our logging service to keep track of what's happening
            LogService = new LoggerService()
            {
                EnableLog = true
            };

            // Get a fresh SMTP client ready for action
            LogService.Info($"SMTPService initialized for server: {smtpSettings.SmtpServerName}"); //** Added logging for testing only
            MailServer = GetMailServerProxy();
           // MailServer.MessageSent
        }

        // The main method that does the heavy lifting of sending an email
        public async Task SendMailAsync(string from, List<string> to, List<string> cc, List<string> bcc,
            string subject, string body, List<string> filenames, MailSignature mailSignature = null, bool deletefilenames = true)
        {
            LogService.Info($"Starting email send process: {subject}"); //** added logging for testing only

            //Need an email body
            if (string.IsNullOrEmpty(body))
            {
                LogService.Error("Email body cannot be null or empty"); //** added logging for testing only
                throw new ArgumentException("Email body cannot be null or empty", nameof(body)); 
            
            }

            // Safety check - make sure the mail server is ready to go
            if (MailServer == null)

            {
                LogService.Error("MailServer not started");
                throw new Exception("MailServer not started");

            }
            LogService.Debug($"Recipients - To: {to?.Count ?? 0}, CC: {cc?.Count ?? 0}, BCC: {bcc?.Count ?? 0}");


            // Connect to the SMTP server if we're not already connected
            if (!MailServer.IsConnected)
            {
                LogService.Info($"Connecting to SMTP server: {SMTPSettings.SmtpServerName}:{SMTPSettings.SmtpServerPort}"); //** Log service used for testing only
                // Set up the connection with appropriate security settings
                await MailServer.ConnectAsync(
                    SMTPSettings.SmtpServerName,
                    Convert.ToInt32(SMTPSettings.SmtpServerPort),
                    SMTPSettings.SmtpServerEnableTSL ?
                        MailKit.Security.SecureSocketOptions.StartTls :  // Use encryption if enabled
                        MailKit.Security.SecureSocketOptions.None        // Plain connection otherwise
                );
                LogService.Info("SMTP connection established successfully");
            }

            // Start building the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(SenderDisplayName, from));  // Who it's from

            // Here's where we handle recipients differently based on whether we're debugging
            if (Debugger.IsAttached)
            {
                // DEBUG MODE: Send ONLY to a single debug BCC address
                message.To.Clear();     
                message.Cc.Clear();       
                message.Bcc.Clear();    // Clear all  recipients done

                // Send ONLY to our special debug BCC address
                var debugBccAddress = "test@example.com";  // Your test email address
                message.Bcc.Add(MailboxAddress.Parse(debugBccAddress));

                // Optional: Log what we're doing for clarity
                LogService.Info($"DEBUG MODE: Sending only to debug BCC: {debugBccAddress}");
            }
            else
            {
                LogService.Info("PRODUCTION MODE: Processing recipients");
                // PRODUCTION MODE: (Keep EXACTLY as is)
                // to Keep an eye on emails in production without affecting the actual recipients
                if (bcc == null)
                {
                    bcc = new List<string>();
                    LogService.Debug("BCC list was null - created empty list");
                }

                if (!string.IsNullOrEmpty(SMTPSettings.DebugBcc))
                {
                    bcc.Add(SMTPSettings.DebugBcc);  // Add our monitoring address
                    LogService.Info($"Added monitoring BCC: {SMTPSettings.DebugBcc}");
                }
                else
                {
                    // Let us know if we forgot to set up the monitoring address
                    LogService.Warn("DebugBcc is not configured - no monitoring emails will be sent");
                }

                // Add all the actual recipients the user specified
                // Add recipients with logging
                to?.ForEach(p => {
                    message.To.Add(MailboxAddress.Parse(p));
                    LogService.Debug($"Added TO recipient: {p}");
                });
                cc?.ForEach(p => {
                    message.Cc.Add(MailboxAddress.Parse(p));
                    LogService.Debug($"Added CC recipient: {p}");
                });
                bcc?.ForEach(p => {
                    message.Bcc.Add(MailboxAddress.Parse(p));
                    LogService.Debug($"Added BCC recipient: {p}");
                });
            }

            // Safety -- Make sure you actually sending to someone!
            if (message.To.Count == 0 && message.Cc.Count == 0 && message.Bcc.Count == 0)
            {
                LogService.Error("No recipients specified for email");
                throw new InvalidOperationException("No recipients have been specified - who are we sending this to?");
            }

            // Set the subject line - make it attention-grabbing!
            message.Subject = subject;

            // Replace template placeholders with actual values
            // Note: Currently using test values - need to use real data in production******************
            body = body.Replace("{email}", "recipient@example.com")
                      .Replace("{resetLink}", "https://example.com/reset")
                      .Replace("{expirationInMinutes}", "60");
            LogService.Debug("Placeholders replaced in email body");

            // Build the email body with HTML content
            var builder = new BodyBuilder();
            builder.HtmlBody = body;  // Fancy HTML 

            // Add any file attachments the user requested
            //if (filenames != null)
            //{
            //    foreach (var file in filenames)
            //    {
            //        if (File.Exists(file))
            //            builder.Attachments.Add(file);  // Attach each valid file
            //    }
            //}

            if (filenames != null)
            {
                int attachedCount = 0;
                foreach (var file in filenames)
                {
                    if (File.Exists(file))
                    {
                        builder.Attachments.Add(file);
                        attachedCount++;
                        LogService.Debug($"Attached file: {file}");
                    }
                    else
                    {
                        LogService.Warn($"File not found, skipping: {file}");
                    }
                }
                LogService.Info($"Attached {attachedCount} file(s)");
            }


            // Put it all together into the final message
            message.Body = builder.ToMessageBody();

            // Time to send the email
            try
            {
                LogService.Info($"Email Sent Start: {message.Subject}");
                await MailServer.SendAsync(message);  // Do the actual sending
                LogService.Info("Email sent successfully", ConsoleColor.Green);
                await MailServer.DisconnectAsync(true);  // Clean up our connection
                LogService.Info("SMTP connection closed");
            }
            catch (Exception ex)
            {
                // something went wrong - log the error and notify
                LogService.Error($"ERROR sending: {message.Subject}");
                Console.WriteLine($"ERROR: {ex.Message}");  // console notification
                throw;  // Re-throw so the caller knows something failed
            }
            finally
            {
                // Cleanup: Delete attached files if requested
                // remove files that shouldn't stick around
                if (deletefilenames && filenames != null)
                    filenames.ForEach(p => { if (File.Exists(p)) File.Delete(p); });
            }
        }

        // Simple method to get a fresh SMTP client
        
        private SmtpClient GetMailServerProxy()
        {
            return new SmtpClient();  
        }
    }
}
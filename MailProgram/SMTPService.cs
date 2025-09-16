using MailKit.Net.Smtp;
using MimeKit;
using System.Diagnostics;
using System.Security.Principal;

namespace MailProgram
{
    //Program flow: Constructor → SendMailAsync (main) → Helper method (AddSignatureToMessage) (GetMailServerProxy) → Send → Cleanup
    public class SMTPService
    {
        public LogService LogService { get; private set; }
        public SmtpClient MailServer { get; private set; }
        public SMTPSettingsDTO SMTPSettings { get; private set; }
        public string SenderDisplayName { get; set; }

        public SMTPService(SMTPSettingsDTO smtpSettings, string senderDisplayName = null)
        {
            /*Constructor Parameters: SMTP settings + optional display name Purpose: Initializes service with configuration and logging*/
            if (senderDisplayName == null) senderDisplayName = smtpSettings.SmtpUser;
            SMTPSettings = smtpSettings;
            SenderDisplayName = senderDisplayName;
            LogService = new LogService()
            {
                IsEnabled = true,
                ApplicationProductName = "Falcon",
                UserId = WindowsIdentity.GetCurrent().Name
            };
            MailServer = GetMailServerProxy();
        }


        //SendMailAsync below: Validates recipients Connects to SMTP server Builds email message Adds attachments/signature Sends email Cleans up file
        public async Task SendMailAsync(string from, List<string> to, List<string> cc, List<string> bcc,
            string subject, string body, List<string> filenames, MailSignature mailSignature = null, bool deletefilenames = true) 
        {
            /* Parameters: from: Sender's email address, to/cc/bcc: Recipient lists, subject/body: Email content, filenames: Attachments

            mailSignature: signature template, deletefilenames: delete file names if requested*/
            // Debug -- Ensure at least one recipient exists when debugger is attached
            if (Debugger.IsAttached && (to == null || to.Count == 0))
                to = new List<string> { "default@example.com" };

            if (MailServer == null) throw new Exception("MailServer not started");

            // Connect to SMTP server if not already connected (using MailPit defaults)
            if (!MailServer.IsConnected)
            {
                //await MailServer.ConnectAsync("localhost", 1025, MailKit.Security.SecureSocketOptions.None);//******* Hard coded for testing only
                await MailServer.ConnectAsync(SMTPSettings.SmtpServerName,Convert.ToInt32(SMTPSettings.SmtpServerPort),SMTPSettings.SmtpServerEnableTSL ?              // SSL/TLS option
                MailKit.Security.SecureSocketOptions.StartTls :MailKit.Security.SecureSocketOptions.None);
            }

            var message = new MimeMessage();
           // message.From.Add(new MailboxAddress(SenderDisplayName, SMTPSettings.SmtpUser));

            // Add all recipients (To, CC, BCC) without debugger checks
            message.From.Add(new MailboxAddress(SenderDisplayName, from));
            if (to != null)
                to.ForEach(p => message.To.Add(MailboxAddress.Parse(p)));
            if (cc != null)
                cc.ForEach(p => message.Cc.Add(MailboxAddress.Parse(p)));
            if (bcc != null)
                bcc.ForEach(p => message.Bcc.Add(MailboxAddress.Parse(p)));

            message.Subject = subject;

            body = body.Replace("{email}", "recipient@example.com") //******hardcoded for testing. use actual values from parameters.
                      .Replace("{resetLink}", "https://example.com/reset")
                      .Replace("{expirationInMinutes}", "60");
                      
            var builder = new BodyBuilder();
            builder.HtmlBody = body;  // HTML email body

            // Replace main email placeholders
           

            // Process file attachments
            if (filenames != null)
            {
                foreach (var file in filenames)
                {
                    if (File.Exists(file))
                        builder.Attachments.Add(file);
                }
            }

            // Add email signature if provided
            if (mailSignature != null)
                AddSignatureToMessage(builder, mailSignature);

            message.Body = builder.ToMessageBody();

            try
            {
                LogService.Info($"Email Sent Start: {message.Subject}");
                await MailServer.SendAsync(message);  // Send email using MailKit
                await MailServer.DisconnectAsync(true);  // New - Disconnect after sending
            }
            catch (Exception ex)
            {
                LogService.Info($"ERROR sending: {message.Subject}");
                Console.WriteLine($"ERROR: {ex.Message}");
                throw;
            }
            finally
            {
                // Cleanup: Delete attached files if requested
                if (deletefilenames && filenames != null)
                    filenames.ForEach(p => { if (File.Exists(p)) File.Delete(p); });
            }
        }

       

        private void AddSignatureToMessage(BodyBuilder builder, MailSignature signature)
        {
            var signatureHTML = signature.SignatureHTML;
            signatureHTML = signatureHTML.Replace("{SENDER_NAME}", signature.SenderName);
            signatureHTML = signatureHTML.Replace("{SENDER_JOBTITLE}", signature.SenderJobTitle);
            
            signatureHTML = signatureHTML.Replace("{SENDER_DEPAREMENT}", signature.SenderDepartment);

            if (!string.IsNullOrEmpty(signature.SignatureLogoFilePath) && File.Exists(signature.SignatureLogoFilePath))
            {
                var image = builder.LinkedResources.Add(signature.SignatureLogoFilePath);
                image.ContentId = Path.GetFileName(signature.SignatureLogoFilePath).Replace(".", "");
                signatureHTML = signatureHTML.Replace("{MAIL_LOGO}", "cid:" + image.ContentId);
            }
            else
            {
                signatureHTML = signatureHTML.Replace("{MAIL_LOGO}", "");
            }

            
            builder.HtmlBody = builder.HtmlBody.Replace("{SIGNATURE}", signatureHTML);
        }


        private SmtpClient GetMailServerProxy()
        {
            // Creates fresh SMTP client instance, Returns new MailKit SmtpClient instance (connection handled in SendMailAsync)
            return new SmtpClient();
        }
    }
}
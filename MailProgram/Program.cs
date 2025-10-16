namespace MailProgram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var smtpSettings = new SMTPSettingsDTO
            {
                
                SmtpServerUrl = "localhost:1025",
                SmtpServerEnableTSL = false,
                SmtpUser = "",
                SmtpPassword = "",
                DebugBcc = "lfranco@fimarge.com"  // Monitoring copy. Production
            };

            var smtpService = new SMTPService(smtpSettings);
            try
            {
                var signature = new MailSignature()
                {
                    SenderName = "Ven",
                    SenderDepartment = "IT",
                    SenderJobTitle = "Dev",
                    SignatureHTML = Content.HTMLSignature,
                    
                };
                await smtpService.SendMailAsync(
                    from: "hello@hello.com",                            //Return address
                    to: new List<string> { "mynewguy@newplace.com" },   //main recipient
                    cc: null,                                           //no CC
                    bcc: new List<string> { "abc@def.com"},             //BCC
                    subject: "Welcome to Fimarge",                      //subject                  
                    body: Content.HTMLBody,                             //Actual email content 
                    //mailSignature: signature,
                    filenames: new List<string> { @"Pack1019.pdf", @"Pack1052.pdf", @"Pack2021.pdf" },
                    
                    deletefilenames: false
                );
                //Console.WriteLine("Email sent successfully");//success
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}"); // Failure
            }
            finally
            {
                
                smtpService.LogService.PrintEmailSummary();

                // Optional: Also get the full JSON log
                string jsonLog = smtpService.LogService.GetLogContent();
                //Console.WriteLine("\nComplete JSON Log:");
                //Console.WriteLine(jsonLog);
            }
        }



    }
}

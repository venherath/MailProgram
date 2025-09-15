namespace MailProgram
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var smtpSettings = new SMTPSettingsDTO
            {
                SmtpServerUrl = "localhost:1025",
                //SmtpServerPort = "1025",
                SmtpServerEnableTSL = false,
                SmtpUser = "",
                SmtpPassword = ""
            };

            var smtpService = new SMTPService(smtpSettings);
            try
            {
                var signature = new MailSignature()
                {
                    SenderName = "Ven",
                    SenderDepartment = "IT",
                    SenderJobTitle = "Dev",
                    SignatureHTML = Content.HTMLSignature
                };
                await smtpService.SendMailAsync(
                    from: "hello@hello.com",
                    to: new List<string> { "mynewguy@newplace.com" },
                    cc: null,
                    bcc: null,
                    subject: "Welcome to Fimarge",
                    body: Content.HTMLBody,
                    mailSignature: signature,
                    //"<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;\">\r\n  <tr>\r\n    <td align=\"center\">\r\n      <table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color: #ffffff; padding: 40px; border-radius: 8px;\">\r\n        <tr>\r\n          <td align=\"center\" style=\"padding-bottom: 30px;\">\r\n            <h1 style=\"color: #333333; margin: 0; font-size: 24px;\">Welcome to Fimarge</h1>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"color: #555555; font-size: 16px; line-height: 1.6;\">\r\n            <p>Dear [First Name],</p>\r\n            <p>It’s our pleasure to welcome you to <strong>Fimarge</strong>, your trusted wealth management partner nestled in the heart of the Pyrenees. For over 30 years, we’ve been helping investors grow, preserve, and protect their wealth through tailor-made strategies, expert guidance, and unwavering commitment.</p>\r\n\r\n            <p>Founded on the principles of transparency, independence, and integrity, Fimarge was created to answer the financial questions we all ask ourselves: <em>How can I grow my wealth?</em> <em>How can I protect it?</em> Our mission has always been clear — to walk the way together with you.</p>\r\n\r\n            <p>Here’s what you can expect as part of the Fimarge community:</p>\r\n            <ul style=\"padding-left: 20px;\">\r\n              <li>State-of-the-art wealth management strategies designed around your goals</li>\r\n              <li>Truly independent advice — no conflicts of interest</li>\r\n              <li>Full transparency at every stage of your journey</li>\r\n              <li>Direct access to our team of top-tier experts</li>\r\n              <li>Personalized service that adapts to your needs over time</li>\r\n            </ul>\r\n\r\n            <p>We’re proud to be the benchmark in Andorra, and we're excited to support your financial journey with the care and attention you deserve.</p>\r\n\r\n            <p>To begin your journey with us, simply click the button below:</p>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td align=\"center\" style=\"padding: 20px 0;\">\r\n            <a href=\"[CTA Link]\" style=\"background-color: #007BFF; color: #ffffff; padding: 12px 24px; border-radius: 4px; text-decoration: none; font-size: 16px;\">Start Now</a>\r\n          </td>\r\n        </tr>\r\n        <tr>\r\n          <td style=\"color: #999999; font-size: 12px; text-align: center; padding-top: 30px;\">\r\n            <p>If you didn’t sign up for this service, you can safely ignore this email.</p>\r\n            <p>&copy; 2025 Fimarge. All rights reserved.</p>\r\n          </td>\r\n        </tr>\r\n      </table>\r\n    </td>\r\n  </tr>\r\n</table>\r\n",
                    //filenames: null
                    filenames: new List<string> { @"Pack1019.pdf", @"Pack1052.pdf", @"Pack2021.pdf" },
                    
                    deletefilenames: false
                );
                Console.WriteLine("Email sent successfully");//success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}"); // Failure
            }
        }
    }
}

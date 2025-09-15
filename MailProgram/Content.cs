namespace MailProgram
{
    public class Content
    {
        public const string HTMLSignature =
            """
            <p style="font-family: Calibri, Helvetica, sans-serif; font-size: 11pt; color: #113475;">
            
              <b>{SENDER_NAME}</b><br>
              {SENDER_JOBTITLE} | {SENDER_DEPAREMENT}<br>
              FIMARGE, SOCIETAT FINANCERA D’INVERSIÓ, SA
            </p>
            <p style="font-family: Calibri, Helvetica, sans-serif; font-size: 10pt; color: #000000;">
              Bonaventura Armengol 10, Bloc 1, 5 PL<br>
              AD500 Andorra la Vella – Principat d’Andorra<br>
              +376 805 100 | <a href="http://www.fimarge.com" target="_blank" rel="noopener" style="color: inherit; text-decoration: none;">fimarge.com</a>
            </p>
            <p>
            <a href="https://www.linkedin.com/company/fimarge" target="_blank" rel="noopener"
               style="text-decoration:none; border:0;">
              <img src="https://cdn-icons-png.flaticon.com/32/174/174857.png" 
                   alt="LinkedIn" width="20" height="20" style="border:0; margin-right:6px;">
            </a>
            <a href="https://www.instagram.com/fimarge/" target="_blank" rel="noopener"
               style="text-decoration:none; border:0;">
              <img src="https://cdn-icons-png.flaticon.com/32/2111/2111463.png" 
                   alt="Instagram" width="20" height="20" style="border:0; margin-right:6px;">
            </a>
            <a href="https://www.facebook.com/fimarge/" target="_blank" rel="noopener"
               style="text-decoration:none; border:0;">
              <img src="https://cdn-icons-png.flaticon.com/32/733/733547.png" 
                   alt="Facebook" width="20" height="20" style="border:0;">
            </a>
            </p>
            <!-- Disclaimer -->
                <table border="0" cellpadding="0" cellspacing="0" style="font-family:Arial,Helvetica,sans-serif;margin-top:20px;">
                    <tbody>
                        <tr>
                            <td>
                                <p
                                    style="margin: 0px; font-family: Calibri, Helvetica, sans-serif; font-size: 9px; color: #666666; line-height: 12px;">
                                    Aquest missatge pot contenir informació confidencial, sotmesa al secret professional o que la
                                    seva divulgació estigui penada per la llei. Si vostè no és el destinatari del missatge, per
                                    favor elimini'l i notifiqui'ns-ho immediatament, no el reenvií ni copií el contingut. Si la seva
                                    empresa no permet la recepció de missatges d'aquest tipus, per favor faci'ns-ho saber
                                    immediatament. El correu electrònic via Internet no permet assegurar la confidencialitat dels
                                    missatges que es transmeten ni la seva integritat o correcta recepció. FIMARGE S.A. no assumeix
                                    responsabilitat per aquestes circumstàncies. Si el destinatari d'aquest missatge no consentís la
                                    utilització del correu electrònic vis Internet i la gravació dels missatges, preguem ho posi en
                                    coneixement nostre de forma immediata.
                                </p>
                                <br>
                                <p
                                    style="margin: 0px; font-family: Calibri, Helvetica, sans-serif; font-size: 9px; color: #666666; line-height: 12px;">
                                    Privileged/Confidential Information may be contained in this message, and protected by a
                                    professional privilege or whose disclosure by a law. If you are not the addressee indicated in
                                    this message you may not copy or deliver this message to anyone. In such case, you should
                                    destroy this message, and notify us immediately. Internet e-mail neither guarantees the
                                    confidentiality nor the integrity or proper receipt of the messages sent. FIMARGE S.A. does not
                                    assume any liability for those circumstances. If the addressee of this message does not consent
                                    to the use of Internet e-mail and message recording, please notify us immediately.
                                </p>
                            </td>
                        </tr>
                    </tbody>
                </table>
            """;
        public const string HTMLBody =
            """
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8'>
                <title>Reset Your Password</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        max-width: 900px;
                        margin: 0 auto;
                        padding: 20px;
                    }}
                    .container {{
                        background-color: #f9f9f9;
                        border-radius: 5px;
                        padding: 20px;
                        border: 1px solid #ddd;
                    }}
                    .button {{
                        display: inline-block;
                        background-color: #0066cc;
                        color: white;
                        padding: 12px 24px;
                        text-decoration: none;
                        border-radius: 4px;
                        margin: 15px 0;
                        font-weight: bold;
                    }}
                    .footer {{
                        margin-top: 20px;
                        font-size: 0.8em;
                        color: #666;
                        border-top: 1px solid #ddd;
                        padding-top: 10px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Password Reset Request</h2>
                    <p>Hello,</p>
                    <p>We received a request to reset the password for your account with email address: <strong>{email}</strong></p>
                    <p>To reset your password, please click the button below:</p>
                    <p><a href='{resetLink}' class='button'>Reset Password</a></p>
                    <p>This password reset link will expire in <strong>{expirationInMinutes} minutes.</strong>.</p>
                    <p>If you did not request a password reset, please ignore this email or contact support if you have concerns.</p>
                </div>
                <div class='footer'>
                    <p>This is an automated email. Please do not reply to this message.</p>
                </div>
                {SIGNATURE}
            </body>
            </html>
            """;
    }
}

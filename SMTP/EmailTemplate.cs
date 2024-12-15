namespace OnlineLibraryAspNet.SMTP
{
    public class EmailTemplate
    {

        public static string GetWelcomeEmailTemplate(string fullname, string username, string password)
        {
            string domain = "ExamSite.uz";


            return $@"<!DOCTYPE html>
          <html lang=""en"">
          <head>
              <meta charset=""UTF-8"">
              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
              <title>Welcome</title>
          </head>
          <body style=""font-family: Arial, sans-serif; background-color: #f0f4f8; margin: 0; padding: 20px;"">
          
              <div style=""background-color: #ffffff; max-width: 500px; width: 100%; padding: 30px; border-radius: 12px; box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1); text-align: center; margin: auto;"">
                  <!-- Header -->
                  <div style=""padding-bottom: 20px; border-bottom: 2px solid #e0e6ed;"">
                      <h1 style=""font-size: 28px; color: #333333; margin: 10px 0;"">Xush kelibsiz!</h1>
                  </div>
          
                  <!-- Body Content -->
                  <div style=""padding: 20px 0;"">
                      <h2 style=""font-size: 22px; color: #4361ee; margin-bottom: 10px;"">Hurmatli, <span style=""font-weight: bold;"">{fullname}</span></h2>
                      <p style=""font-size: 16px; color: #555555; line-height: 1.6;"">
                          Bizning xizmatlarimizga qo‘shilganingiz uchun rahmat! <br> Quyida kirish ma'lumotlaringiz keltirilgan:
                      </p>
          
                      <!-- User Info -->
                      <div style=""background-color: #f8f9fc; padding: 15px; border-radius: 8px; margin-top: 20px;"">
                          <p style=""margin: 10px 0; font-size: 16px; color: #333333;"">Foydalanuvchi nomi: <strong>{username}</strong></p>
                          <p style=""margin: 10px 0; font-size: 16px; color: #333333;"">Parol: <strong>{password}</strong></p>
                      </div>
          
                      <!-- Call to Action -->
                      <a href=""https://ExamSite.uz"" style=""display: inline-block; margin-top: 30px; padding: 12px 24px; font-size: 16px; color: #ffffff; background-color: #4361ee; border-radius: 8px; text-decoration: none;"">
                          {domain} ga kirish
                      </a>
                  </div>
          
                  <!-- Footer -->
                  <div style=""padding-top: 20px; border-top: 2px solid #e0e6ed; margin-top: 20px; color: #777777; font-size: 14px;"">
                      &copy; {DateTime.Now.Year.ToString()} {domain}. Barcha huquqlar himoyalangan.
                  </div>
              </div>
          </body>
          </html>
          ";

        }

        public static string GetVerificationEmailTemplate(string fullname, string verificationCode)
        {
            string domain = "ExamSite.uz";

            return $@"
                          <!DOCTYPE html>
                          <html lang=""en"">
                          <head>
                              <meta charset=""UTF-8"">
                              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                              <title>Tasdiqlash Kodi</title>
                              <style>
                                  body {{
                                      font-family: Arial, sans-serif;
                                      background-color: #f0f4f8;
                                      margin: 0;
                                      padding: 0;
                                      display: flex;
                                      justify-content: center;
                                      align-items: center;
                                      height: 100vh;
                                  }}
                      
                                  .container {{
                                      background-color: #ffffff;
                                      max-width: 500px;
                                      width: 100%;
                                      padding: 30px;
                                      border-radius: 12px;
                                      box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
                                      text-align: center;
                                  }}
                      
                                  .header h2 {{
                                      font-size: 24px;
                                      color: #333333;
                                      margin-bottom: 20px;
                                  }}
                      
                                  .content p {{
                                      font-size: 16px;
                                      color: #555555;
                                      line-height: 1.6;
                                      margin: 15px 0;
                                  }}
                      
                                  .highlight {{
                                      font-size: 20px;
                                      font-weight: bold;
                                      color: #4361ee;
                                  }}
                      
                                  .footer {{
                                      padding-top: 20px;
                                      border-top: 2px solid #e0e6ed;
                                      margin-top: 20px;
                                      color: #777777;
                                      font-size: 14px;
                                  }}
                      
                                  @media (max-width: 480px) {{
                                      .container {{
                                          padding: 20px;
                                          width: 90%;
                                      }}
                      
                                      .header h2 {{
                                          font-size: 22px;
                                      }}
                      
                                      .content p {{
                                          font-size: 14px;
                                      }}
                      
                                      .highlight {{
                                          font-size: 18px;
                                      }}
                      
                                      .footer {{
                                          font-size: 12px;
                                      }}
                                  }}
                              </style>
                          </head>
                          <body>
                      
                              <div class=""container"">
                                  <div class=""header"">
                                      <h2>Hurmatli, {fullname}</h2>
                                  </div>
                      
                                  <div class=""content"">
                                      <p>Sizning hisobni tiklash kodingiz:</p>
                                      <p><span class=""highlight"">{verificationCode}</span></p>
                                      <p>Ushbu kodni hechkimga bermang!</p>
                                      <p>Kodning amal qilish muddati 15 daqiqa.</p>
                                  </div>
                      
                                  <div class=""footer"">
                                       &copy; {DateTime.Now.Year.ToString()} {domain}. Barcha huquqlar himoyalangan.
                                  </div>
                              </div>
                      
                          </body>
                          </html>
                ";
        }
    }
}

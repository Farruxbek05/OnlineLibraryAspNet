using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.IRepositorys;
using OnlineLibraryAspNet.Models;
using System.Net.Mail;
using System.Net;
using OnlineLibraryAspNet.Enum;
using OnlineLibraryAspNet.SMTP;

namespace OnlineLibraryAspNet.Service
{
    public class EmailService:IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IVerificationCodeRepository _verificationCodeRepository;

        public EmailService(IOptions<SmtpSettings> smtpSettings, IPasswordHasher<User> passwordHasher,
                            IUserRepository userRepository, IVerificationCodeRepository verificationCodeRepository)
        {
            _smtpSettings = smtpSettings.Value;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _verificationCodeRepository = verificationCodeRepository;
        }

        public async Task<ResponceDto> SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Server) // Portni o'zgartirish
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                };


                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);


                await smtpClient.SendMailAsync(mailMessage);

                return new ResponceDto
                {
                    Success = true,
                    Message = "Xabar muvaffaqiyatli yuborildi!"
                };
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP xatosi: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");

                return new ResponceDto
                {
                    Success = false,
                    Message = $"SMTP xatosi: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new ResponceDto
                {
                    Success = false,
                    Message = $"Umumiy xato: {ex.Message}"
                };
            }


        }
        public async Task<ResponceDto> SendWelcomeEmailAsync(string toEmail, string fullname, string username, string password)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Server)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = "Xush kelibsiz",
                    Body = EmailTemplate.GetWelcomeEmailTemplate(fullname, username, password),
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

                return new ResponceDto
                {
                    Success = true,
                    Message = "Xabar muvaffaqiyatli yuborildi!"
                };

            }
            catch
            {
                return new ResponceDto
                {
                    Success = false,
                    Message = $"Xabarni yuborishda xatolik!"
                };
            }
        }

        public async Task<ResponceDto> SendVerificationEmailAsync(string toEmail, string verificationCode)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(toEmail);

                if (!user.Success)
                {
                    return new ResponceDto
                    {
                        Success = false,
                        Message = "Email tizimda topilmadi"
                    };
                }

                if (user.Data.IsEmailConfirmed)
                {
                    return new ResponceDto
                    {
                        Success = false,
                        Message = "Bu Email tasdiqlangan"
                    };
                }

                // So'nggi yuborilgan emailni qidirish
                var lastVerification = await _verificationCodeRepository.GetLastVerificationCodeAsync(toEmail, VerificationCodeType.EmailVerificationCode);

                if (lastVerification != null)
                {
                    var reAttempt = (DateTime.UtcNow - lastVerification.Data.CreatedAt).TotalSeconds;

                    if (reAttempt < 60)
                    {
                        return new ResponceDto
                        {
                            Success = false,
                            Message = $"Kodni qayta yuborish uchun {Math.Round(60 - reAttempt)} soniya kutishingiz kerak"
                        };
                    }
                }

                // SMTP Clientni "using" bilan ochish
                using (var smtpClient = new SmtpClient(_smtpSettings.Server)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                })
                {
                    var fullname = $"{user.Data.LastName} {user.Data.FirstName}";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                        Subject = "Tasdiqlash Kodingiz",
                        Body = EmailTemplate.GetVerificationEmailTemplate(fullname, verificationCode),
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                // Verification kodni saqlash
                var verificationRecord = new VerificationCode
                {
                    Email = toEmail,
                    Code = verificationCode,
                    CodeType = VerificationCodeType.EmailVerificationCode,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15), // Kodning amal qilish muddati
                    UserId = user.Data.Id
                };

                var result = await _verificationCodeRepository.CreateAsync(verificationRecord);



                return new ResponceDto
                {
                    Success = true,
                    Message = "Tasdiqlash xabari muvaffaqiyatli yuborildi!"
                };
            }
            catch (Exception ex)
            {
                // Xatolikni loglash yoki tekshirish uchun xatolikni batafsil olish
                return new ResponceDto
                {
                    Success = false,
                    Message = $"Tasdiqlash xabarini yuborishda xatolik: {ex.Message}"
                };
            }
        }


        public async Task<ResponceDto> VerifyEmailAsync(string email, string verificationCode)
        {
            var verificationRecord = await _verificationCodeRepository.GetVerificationCodeAsync(email, verificationCode, VerificationCodeType.EmailVerificationCode);

            if (verificationRecord != null && verificationRecord.Data.ExpiresAt > DateTime.UtcNow)
            {
                var user = await _userRepository.FindByEmailAsync(email);
                if (user != null)
                {
                    user.Data.IsEmailConfirmed = true;

                    await _userRepository.UpdateUserAsync(user.Data.Id, user.Data);
                    await _verificationCodeRepository.DeleteByIdAsync(verificationRecord.Data.Id);

                    return new ResponceDto
                    {
                        Success = true,
                        Message = "Emailni tasdiqlandi."
                    };
                }
            }

            return new ResponceDto
            {
                Success = false,
                Message = "Emailni tasdiqlashda xatolik."
            };
        }

        public string GenerateVerificationCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }

            return new string(code);
        }

        public async Task<ResponceDto> ResetPasswordRequest(string toEmail, string verificationCode)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(toEmail);

                if (!user.Success)
                {
                    return new ResponceDto
                    {
                        Success = false,
                        Message = "Email tizimda topilmadi."
                    };
                }

                var lastVerification = await _verificationCodeRepository.GetVerificationCodeAsync(toEmail, verificationCode, VerificationCodeType.ResetPasswordCode);

                if (lastVerification != null)
                {
                    var reAttempt = (DateTime.UtcNow - lastVerification.Data.CreatedAt).TotalSeconds;

                    if (reAttempt < 60)
                    {
                        return new ResponceDto
                        {
                            Success = false,
                            Message = $"Kodni qayta yuborish uchun {Math.Round(60 - reAttempt)} soniya kutishingiz kerak"
                        };
                    }
                }

                // SMTP Clientni "using" bilan ochish
                using (var smtpClient = new SmtpClient(_smtpSettings.Server)
                {
                    Port = _smtpSettings.Port,
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                })
                {
                    var fullname = $"{user.Data.LastName} {user.Data.FirstName}";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                        Subject = "Parolni tiklash kodini yuborish",
                        Body = EmailTemplate.GetVerificationEmailTemplate(fullname, verificationCode),
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                // Verification kodni saqlash
                var verificationRecord = new VerificationCode
                {
                    Email = toEmail,
                    Code = verificationCode,
                    CodeType = VerificationCodeType.ResetPasswordCode,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    UserId = user.Data.Id
                };

                await _verificationCodeRepository.CreateAsync(verificationRecord);

                if (lastVerification.Success)
                {
                    return new ResponceDto
                    {
                        Success = true,
                        Message = "Parolni tiklash xabari muvaffaqiyatli yuborildi!"
                    };
                }

                return new ResponceDto
                {
                    Success = false,
                    Message = "Parolni tiklash xabari yuborilmadi!"
                };



            }
            catch (Exception ex)
            {
                // Xatolikni loglash yoki tekshirish uchun xatolikni batafsil olish
                return new ResponceDto
                {
                    Success = false,
                    Message = $"Parolni tiklash xabarini yuborishda xatolik: {ex.Message}"
                };
            }
        }


        public async Task<ResponceDto> ResetMyPassword(string Email, string VerificationCode, string Password)
        {
            try
            {
                var verificationRecord = await _verificationCodeRepository.GetVerificationCodeAsync(Email, VerificationCode, VerificationCodeType.ResetPasswordCode);

                if (verificationRecord != null && verificationRecord.Data.ExpiresAt > DateTime.UtcNow)
                {
                    var user = await _userRepository.FindByEmailAsync(Email);
                    if (user.Success)
                    {
                        user.Data.HashPassword = _passwordHasher.HashPassword(new User(), Password);

                        await _userRepository.UpdateUserAsync(user.Data.Id, user.Data);

                        await _verificationCodeRepository.DeleteByIdAsync(verificationRecord.Data.Id);

                        return new ResponceDto
                        {
                            Success = true,
                            Message = "Parol muvaffaqiyatli o'zgartirildi."
                        };
                    }
                }

                return new ResponceDto
                {
                    Success = false,
                    Message = "Parolni tiklashda xatolik."
                };
            }
            catch (Exception ex)
            {
                return new ResponceDto
                {
                    Success = false,
                    Message = $"Parolni tiklashda xatolik: {ex.Message}"
                };
            }
        }
    }
}


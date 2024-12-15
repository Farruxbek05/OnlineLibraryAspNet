using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using OnlineLibraryAspNet.Enum;

namespace OnlineLibraryAspNet.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Ismingizni kiritishingiz shart.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Ism uzunligi kamida 2 va maksimal 15 ta belgidan iborat bo'lishi kerak.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Ism faqat harflar va bo'sh joylardan iborat bo'lishi kerak.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Familiyangizni kiritishingiz shart.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Familiya uzunligi kamida 3 va maksimal 15 ta belgidan iborat bo'lishi kerak.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Familiya faqat harflar va bo'sh joylardan iborat bo'lishi kerak.")]
        public string LastName { get; set; }

        [StringLength(25, MinimumLength = 2, ErrorMessage = "Otasining ismi uzunligi kamida 2 va maksimal 25 ta belgidan iborat bo'lishi kerak.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Otasining ismi faqat harflar va bo'sh joylardan iborat bo'lishi kerak.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Foydalanuvchi nomini kiritish shart.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Foydalanuvchi nomi min 5 tadan, max 20 tagacha bo'lgan belgilardan iborat bo'lishi kerak.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Foydalanuvchi nomida faqat harflar va raqamlar bo'lishi mumkin.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email manzilini kiritish shart.")]
        [EmailAddress(ErrorMessage = "Email manzil noto'g'ri formatda.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Jinsni kiritish shart. 0 - Erkak, 1 - Ayol")]
        [Range(1, 2, ErrorMessage = "Qiymat faqat 0 yoki 1 bo'lishi kerak")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Parol kiritilishi shart.")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Parol kamida 8 ta belgidan iborat bo'lishi kerak. Max: 15")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Parol kamida 1 ta kichik harf, 1 ta katta harf, 1 ta belgi va 1 ta raqamdan iborat bo'lishi kerak.")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}

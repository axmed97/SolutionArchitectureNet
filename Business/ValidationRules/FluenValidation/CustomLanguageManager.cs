using FluentValidation.Resources;

namespace Business.ValidationRules.FluenValidation
{
    public class CustomLanguageManager : LanguageManager
    {
        public CustomLanguageManager()
        {
            #region Register
            // Translations for az (Azerbaijani)
            AddTranslation("az", "EmailIsRequired", "Email boş ola bilməz!");
            AddTranslation("az", "EmailIsValid", "Etibarlı bir e-poçt ünvanı tələb olunur!");
            AddTranslation("az", "UsernameIsRequired", "İstifadəçi adı boş ola bilməz!");
            AddTranslation("az", "UsernameTooShort", "İstifadəçi adı ən azı 6 simvol olmalıdır!");
            AddTranslation("az", "FirstnameIsRequired", "Ad boş ola bilməz!");
            AddTranslation("az", "FirstnameNoDigits", "Adınızda rəqəm olə bilməz!");
            AddTranslation("az", "LastnameIsRequired", "Soyad boş ola bilməz!");
            AddTranslation("az", "LastnameNoDigits", "Soyadınızda rəqəm ola bilməz!");
            AddTranslation("az", "PasswordIsRequired", "Şifrə boş ola bilməz!");
            AddTranslation("az", "ConfirmPasswordIsRequired", "Şifrəni təsdiqlə boş ola bilməz!");
            AddTranslation("az", "PasswordsMustMatch", "Şifrələr uyğun gəlmir!");

            // Translations for ru-RU (Russian)
            AddTranslation("ru-RU", "EmailIsRequired", "Email не может быть пустым!");
            AddTranslation("ru-RU", "EmailIsValid", "Требуется действительный адрес электронной почты!");
            AddTranslation("ru-RU", "UsernameIsRequired", "Имя пользователя не может быть пустым!");
            AddTranslation("ru-RU", "UsernameTooShort", "Имя пользователя должно быть не менее 6 символов!");
            AddTranslation("ru-RU", "FirstnameIsRequired", "Имя не может быть пустым!");
            AddTranslation("ru-RU", "FirstnameNoDigits", "Имя не должно содержать цифры!");
            AddTranslation("ru-RU", "LastnameIsRequired", "Фамилия не может быть пустой!");
            AddTranslation("ru-RU", "LastnameNoDigits", "Фамилия не должна содержать цифры!");
            AddTranslation("ru-RU", "PasswordIsRequired", "Пароль не может быть пустым!");
            AddTranslation("ru-RU", "ConfirmPasswordIsRequired", "Подтверждение пароля не может быть пустым!");
            AddTranslation("ru-RU", "PasswordsMustMatch", "Пароли должны совпадать!");

            // Translations for en-US (English)
            AddTranslation("en-US", "EmailIsRequired", "Email can't be empty!");
            AddTranslation("en-US", "EmailIsValid", "A valid email address is required!");
            AddTranslation("en-US", "UsernameIsRequired", "Username can't be empty!");
            AddTranslation("en-US", "UsernameTooShort", "Username must be at least 6 characters long!");
            AddTranslation("en-US", "FirstnameIsRequired", "First name can't be empty!");
            AddTranslation("en-US", "FirstnameNoDigits", "First name must not contain digits!");
            AddTranslation("en-US", "LastnameIsRequired", "Last name can't be empty!");
            AddTranslation("en-US", "LastnameNoDigits", "Last name must not contain digits!");
            AddTranslation("en-US", "PasswordIsRequired", "Password can't be empty!");
            AddTranslation("en-US", "ConfirmPasswordIsRequired", "Confirm password can't be empty!");
            AddTranslation("en-US", "PasswordsMustMatch", "Passwords must match!");
            #endregion
        }
    }
}

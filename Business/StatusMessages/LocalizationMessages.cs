namespace Business.Messages
{
    public static class LocalizationMessages
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _messages = new()
        {
            {"az", new Dictionary<string, string>
                {
                    { "UserNotFound", "İstifadəçi tapılmadı!" },
                    { "EmailAlreadyExists", "E-poçt artıq mövcuddur!" },
                    { "RegistrationSuccess", "Uğurla qeydiyyatdan keçdiniz!" },
                }
            },
            {"ru-RU", new Dictionary<string, string>
                {
                    { "UserNotFound", "Пользователь не найден!" },
                    { "EmailAlreadyExists", "Email уже используется!" },
                    { "RegistrationSuccess", "Регистрация прошла успешно!" },

                }
            },
            {"en-US", new Dictionary<string, string>
                {
                    { "UserNotFound", "User not found!" },
                    { "EmailAlreadyExists", "Email is already in use!" },
                    { "RegistrationSuccess", "Registration successful!" },

                }
            }
        };

        public static string GetLocalizedString(string key, string culture)
        {
            if (_messages.TryGetValue(culture, out var localizedMessages) && localizedMessages.TryGetValue(key, out var message))
            {
                return message;
            }
            // Default to Aerbaijan if the specified culture or key is not found
            return _messages["az"][key];
        }
    }
}

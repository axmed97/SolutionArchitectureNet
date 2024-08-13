using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluenValidation
{
    public class BaseAbstractValidator<T> : AbstractValidator<T>
    {
        public string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
        public bool ValidateLangCode(string LangCode)
        {
            var validLangCodes = new[] { "en-US", "zh", "ru-RU" };
            return validLangCodes.Contains(LangCode);
        }
    }
}

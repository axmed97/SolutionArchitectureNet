using Business.Messages.Abstract;

namespace Business.Messages.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        public string GetLocalizedString(string key, string culture)
        {
            return LocalizationMessages.GetLocalizedString(key, culture);
        }
    }
}

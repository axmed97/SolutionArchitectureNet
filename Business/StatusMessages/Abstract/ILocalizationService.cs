namespace Business.Messages.Abstract
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string key, string culture);
    }
}

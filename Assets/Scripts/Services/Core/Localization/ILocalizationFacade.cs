using TMPro;

namespace IdxZero.Services.Localization
{
    public interface ILocalizationFacade
    {
        string GetText(string key);
        void SetTextWithKey(string key, TMP_Text tmpText);
        void SetText(string text, TMP_Text tmpText);
        void SetFontPreset(TMP_Text tmpText);
        bool IsCurrentFontSupportCharacter(char[] chars);
    }
}
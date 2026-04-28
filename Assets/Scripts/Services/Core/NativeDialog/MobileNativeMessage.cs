using System;
using IdxZero.Services.Localization;

namespace IdxZero.Services.NativeDialog
{
    public class MobileNativeMessage : IMobileNativeMessage
    {
        private readonly ILocalizationFacade _localizationFacade;

        public MobileNativeMessage(ILocalizationFacade localizationFacade)
        {
            _localizationFacade = localizationFacade;
        }

        public void ShowOfferToRateAppMessage(Action yesCallback)
        {
            MobileNativeDialog.NativeDialog.OpenDialog("Do you like the app?", null, "No", "Yes", null, yesCallback);
        }
    }

    public interface IMobileNativeMessage
    {
        void ShowOfferToRateAppMessage(Action p);
    }
}
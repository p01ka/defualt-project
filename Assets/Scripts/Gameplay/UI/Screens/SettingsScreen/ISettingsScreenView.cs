using System;

namespace Gameplay.UI.Screens
{
    public interface ISettingsScreenView
    {
        event Action OnCloseButtonPressed;
        event Action<bool> OnMusicTogglePressed;
        event Action<bool> OnSoundTogglePressed;
        event Action<bool> OnVibrationTogglePressed;
        event Action OnWriteUsButtonPressed;
        event Action OnGetPremiumButtonPressed;
        event Action OnTermsOfUseButtonPressed;
        event Action OnPrivacyPolicyButtonPressed;

        void ActivatePanel(bool active);
        void SetMusicStatus(bool isOn);
        void SetSoundStatus(bool isOn);
        void SetVibrationStatus(bool isOn);
        void ActiveGetPremiumButton(bool isActive);

        event Action OnShowAdsDebuggerEvent;
    }
}
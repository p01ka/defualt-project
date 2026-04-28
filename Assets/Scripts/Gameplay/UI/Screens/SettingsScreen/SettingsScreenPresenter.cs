using System;
using IdxZero.Base.MVP;
using IdxZero.Utils;
using UnityEngine;
using Zenject;

namespace Gameplay.UI.Screens
{
    public class SettingsScreenPresenter : BasePresenter
    {
        private readonly ISettingsScreenView _view;
        private readonly SignalBus _signals;

        public const string RecipientAddressEmail = "support@exomind.gg";

        public SettingsScreenPresenter(ISettingsScreenView view,
                                       SignalBus signals)
        {
            _view = view;
            _signals = signals;
            _signals.Subscribe<SettingsPanelSignals.OnOpenSettingsPanel>(ShowPanel);
            SetStartSettings();
        }

        private void SetStartSettings()
        {
            SetMusicStatus();
            SetSoundStatus();
            SetVibrationStatus();
            ClosePanel();
        }

        private void ShowPanel()
        {
            _view.ActivatePanel(true);
        }

        private void ClosePanel()
        {
            _view.ActivatePanel(false);
        }

        private void SetMusicStatus()
        {
            int currentMusicStatus = PlayerPrefs.GetInt(PrefConstKeys.MusicStatusKey, 1);
            _view.SetMusicStatus(currentMusicStatus == 1);
        }

        private void SetVibrationStatus()
        {
            int currentVibrationStatus = PlayerPrefs.GetInt(PrefConstKeys.VibrationStatusKey, 1);
            _view.SetVibrationStatus(currentVibrationStatus == 1);
        }

        private void SetSoundStatus()
        {
            int currentSoundStatus = PlayerPrefs.GetInt(PrefConstKeys.SoundStatusKey, 1);
            _view.SetSoundStatus(currentSoundStatus == 1);
        }

        private void MusicStatusChanged(bool isOn)
        {
            int newMusicStatus = isOn ? 1 : 0;
            PlayerPrefs.SetInt(PrefConstKeys.MusicStatusKey, newMusicStatus);
            //_signals.TryFire(new AudioHapticSignals.OnUserChangeAudioHapticStatus(true));
        }

        private void VibrationStatusChanged(bool isOn)
        {
            int newVibrationStatus = isOn ? 1 : 0;
            PlayerPrefs.SetInt(PrefConstKeys.VibrationStatusKey, newVibrationStatus);
            //_signals.TryFire<AudioHapticSignals.OnUserChangeAudioHapticStatus>();
        }

        private void SoundStatusChanged(bool isOn)
        {
            int newSoundStatus = isOn ? 1 : 0;
            PlayerPrefs.SetInt(PrefConstKeys.SoundStatusKey, newSoundStatus);
            //_signals.TryFire<AudioHapticSignals.OnUserChangeAudioHapticStatus>();
        }

        private void ContactUs()
        {
            string platform = Enum.GetName(typeof(RuntimePlatform), UnityEngine.Application.platform);
            var title = $"Feedback about {UnityEngine.Application.productName} app // Version {UnityEngine.Application.version} // Platform {platform}";

            var message = "Your opinion is important to us!";
#if UNITY_IOS
            title = title.Replace(" ", "%20");
            message = message.Replace(" ", "%20");
#endif
            var mailUrl = $"mailto:{RecipientAddressEmail}?subject={title}&body={message}";
            UnityEngine.Application.OpenURL(mailUrl);
        }

        private void OpenTermsOfUse()
        {
            UnityEngine.Application.OpenURL(Consts.TermsOfUseUrl);
        }

        private void OpenPrivacyPolicy()
        {
            UnityEngine.Application.OpenURL(Consts.PrivacyPolicyUrl);
        }

        private void TryToShowAdsDebugger()
        {
        }

        private void OnNoAdsUserStatusChanged(bool isNoAdsUser)
        {
            _view.ActiveGetPremiumButton(!isNoAdsUser);
        }

        protected override void SubscribeOnViewEvents()
        {
            _view.OnCloseButtonPressed += ClosePanel;

            _view.OnMusicTogglePressed += MusicStatusChanged;
            _view.OnVibrationTogglePressed += VibrationStatusChanged;
            _view.OnSoundTogglePressed += SoundStatusChanged;

            _view.OnWriteUsButtonPressed += ContactUs;
            _view.OnTermsOfUseButtonPressed += OpenTermsOfUse;
            _view.OnPrivacyPolicyButtonPressed += OpenPrivacyPolicy;
            _view.OnShowAdsDebuggerEvent += TryToShowAdsDebugger;

        }

        protected override void UnsubscribeOnViewEvents()
        {
            _view.OnCloseButtonPressed -= ClosePanel;

            _view.OnMusicTogglePressed -= MusicStatusChanged;
            _view.OnVibrationTogglePressed -= VibrationStatusChanged;
            _view.OnSoundTogglePressed -= SoundStatusChanged;

            _view.OnWriteUsButtonPressed -= ContactUs;
            _view.OnTermsOfUseButtonPressed -= OpenTermsOfUse;
            _view.OnPrivacyPolicyButtonPressed -= OpenPrivacyPolicy;
            _view.OnShowAdsDebuggerEvent -= TryToShowAdsDebugger;
        }

        public override void Dispose()
        {
            _signals.TryUnsubscribe<SettingsPanelSignals.OnOpenSettingsPanel>(ShowPanel);
        }

        #region Factory

        public class Factory : PlaceholderFactory<ISettingsScreenView, BasePresenter>
        {
        }

        #endregion Factory
    }
}
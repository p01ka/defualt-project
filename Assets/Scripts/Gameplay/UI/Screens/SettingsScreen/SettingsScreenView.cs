using System;
using IdxZero.Base.MVP;
using IdxZero.UIGeneral.Switcher;
using IdxZero.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI.Screens
{
    public class SettingsScreenView : AnimatedShowingBaseView, ISettingsScreenView
    {
#pragma warning disable 0649
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _writeUsButton;
        [SerializeField] private CustomToggle _musicToggle;
        [SerializeField] private CustomToggle _soundToggle;
        [SerializeField] private CustomToggle _vibrationToggle;
        [SerializeField] private Button _termsOfUseButton;
        [SerializeField] private Button _privacyPolicyButton;
#pragma warning restore 0649

        public event Action OnCloseButtonPressed;
        public event Action<bool> OnMusicTogglePressed;
        public event Action<bool> OnSoundTogglePressed;
        public event Action<bool> OnVibrationTogglePressed;
        public event Action OnWriteUsButtonPressed;
        public event Action OnGetPremiumButtonPressed;
        public event Action OnTermsOfUseButtonPressed;
        public event Action OnPrivacyPolicyButtonPressed;
        public event Action OnShowAdsDebuggerEvent;
        private float _showAdsDebuggerButtonPointerDownTime;
        private const float ShowAdsDebuggerOffsetInSec = 7f;

        private BasePresenter _presenter;
        // private AudioEffectHandler _audioEffectHandler;

        [Inject]
        private void Construct(SettingsScreenPresenterFactory presenterFactory
            // ,AudioEffectHandler audioEffectHandler
                               )
        {
            _presenter = presenterFactory.Create(this);
            // _audioEffectHandler = audioEffectHandler;
            SetPointEvents();
            Hide(false);
        }

        private void SetPointEvents()
        {
            void PointCallback()
            {
                _showAdsDebuggerButtonPointerDownTime = Time.unscaledTime;
            }
            SetPointTrigger(_closeButton.gameObject, PointCallback);
            SetPointTrigger(_writeUsButton.gameObject);
            SetPointTrigger(_musicToggle.gameObject);
            SetPointTrigger(_soundToggle.gameObject);
            SetPointTrigger(_vibrationToggle.gameObject);
            SetPointTrigger(_termsOfUseButton.gameObject);
            SetPointTrigger(_privacyPolicyButton.gameObject);
        }

        private void SetPointTrigger(GameObject go,
                                     Action callback = null)
        {
            EventTrigger trigger = go.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener((e) =>
            {
                PlayButtonSound();
                callback?.Invoke();
            });
            trigger.triggers.Add(pointerDown);
        }

        private void PlayButtonSound()
        {
            Debug.LogWarning($"PlayButtonSound() not implemented!");
            //_audioEffectHandler.PlayTapSoundEffect();
        }

        public void ActivatePanel(bool active)
        {
            if (active)
            {
                Show(true);
            }
            else
            {
                Hide(true);
            }
        }

        public void SetMusicStatus(bool isOn)
        {
            _musicToggle.SetMode(isOn);
        }

        public void SetSoundStatus(bool isOn)
        {
            _soundToggle.SetMode(isOn);
        }

        public void SetVibrationStatus(bool isOn)
        {
            _vibrationToggle.SetMode(isOn);
        }

        public void ActiveGetPremiumButton(bool isActive)
        {
        }

        private void SetActiveMusic(bool isActive)
        {
            OnMusicTogglePressed?.Invoke(isActive);
        }

        public void SetActiveSound(bool isActive)
        {
            OnSoundTogglePressed?.Invoke(isActive);
        }

        public void SetActiveVibro(bool IsActive)
        {
            OnVibrationTogglePressed?.Invoke(IsActive);
        }

        private void CloseButtonPressed()
        {
            float currentTime = Time.unscaledTime;
            float pointerDownTimeOffset = currentTime - _showAdsDebuggerButtonPointerDownTime;
            if (pointerDownTimeOffset > ShowAdsDebuggerOffsetInSec)
            {
                GameObject debugGo = new GameObject("DEBUG OUTPUT CONSOLE");
                debugGo.AddComponent<DebugConsoleOutput>();
                DeviceUtils.IsDebugMode = true;
                OnShowAdsDebuggerEvent?.Invoke();
            }
            else
                OnCloseButtonPressed?.Invoke();
        }

        private void WriteUsButtonPressed()
        {
            OnWriteUsButtonPressed?.Invoke();
        }

        private void GetPremiumButtonPressed()
        {
            OnGetPremiumButtonPressed?.Invoke();
        }

        private void TermsOfUseButtonPressed()
        {
            OnTermsOfUseButtonPressed?.Invoke();
        }

        private void PrivacyPolicyButtonPressed()
        {
            OnPrivacyPolicyButtonPressed?.Invoke();
        }

        private void OnEnable()
        {
            SubscribeOnEvents();
            _presenter.Initialize();
        }

        private void OnDisable()
        {
            UnsubscribeOnEvents();
            _presenter.Uninitialize();
        }

        protected override void SubscribeOnEvents()
        {
            _closeButton.onClick.AddListener(CloseButtonPressed);

            _musicToggle.OnValueChanged += SetActiveMusic;
            _soundToggle.OnValueChanged += SetActiveSound;
            _vibrationToggle.OnValueChanged += SetActiveVibro;

            _writeUsButton.onClick.AddListener(WriteUsButtonPressed);
            _termsOfUseButton.onClick.AddListener(TermsOfUseButtonPressed);
            _privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonPressed);
        }

        protected override void UnsubscribeOnEvents()
        {
            _closeButton.onClick.RemoveListener(CloseButtonPressed);

            _musicToggle.OnValueChanged -= SetActiveMusic;
            _soundToggle.OnValueChanged -= SetActiveSound;
            _vibrationToggle.OnValueChanged -= SetActiveVibro;

            _writeUsButton.onClick.RemoveListener(WriteUsButtonPressed);
            _termsOfUseButton.onClick.RemoveListener(TermsOfUseButtonPressed);
            _privacyPolicyButton.onClick.RemoveListener(PrivacyPolicyButtonPressed);
        }
    }
}
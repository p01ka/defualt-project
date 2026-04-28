using System;
using IdxZero.Base.MVP;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace IdxZero.Gameplay.UI.GameplayButtonsPanel
{
    public class GameplayButtonsPanelView : BaseView, IGameplayButtonsPanelView
    {
#pragma warning disable 0649

        [SerializeField]
        private Button _backButton;
        
        [SerializeField]
        private Button _settingButton;

#pragma warning restore 0649

        public event Action OnBackButtonPressed;
        public event Action OnSettingsButtonPressed;

        [Inject]
        private void Construct(GameplayButtonsPanelPresenterFactory presenterFactory)
        {
            Presenter = presenterFactory.Create(this);
#if RELEASE_MODE
            _backButton.gameObject.SetActive(false);
#endif
        }

        private void CloseButtonPressed()
        {
            OnBackButtonPressed?.Invoke();
        }
        
        private void SettingsButtonPressed()
        {
            OnSettingsButtonPressed?.Invoke();
        }

        protected override void SubscribeOnEvents()
        {
            _backButton.onClick.AddListener(CloseButtonPressed);
            _settingButton.onClick.AddListener(SettingsButtonPressed);
        }

        protected override void UnsubscribeOnEvents()
        {
            _backButton.onClick.RemoveListener(CloseButtonPressed);
            _settingButton.onClick.RemoveListener(SettingsButtonPressed);
        }
    }
}






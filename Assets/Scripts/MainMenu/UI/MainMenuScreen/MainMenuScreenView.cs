using System;
using System.Linq;
using IdxZero.Base.MVP;
using IdxZero.Gameplay.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace IdxZero.MainMenu.UI.MainMenuScreen
{
    public class MainMenuScreenView : BaseView, IMainMenuScreenView
    {
#pragma warning disable 0649
        [SerializeField]
        private Canvas _screenCanvas;

        [SerializeField]
        private Button _startGameplayButton;

        [SerializeField]
        private Button _startGameplayStateConfigButton;

        [SerializeField]
        private TMP_Dropdown _dropdownSelectStateConfig;

        [SerializeField]
        private Button _showAdsDebugButton;

        [SerializeField]
        private Button _clearProgressButton;

        [SerializeField]
        private Toggle _useDebugModeTogle;

        [SerializeField]
        private TMP_InputField _timescaleInputField;
#pragma warning restore 0649

        public event Action OnStartGameplayButtonPressed;
        public event Action OnStartGameplayStateConfigButtonPressed;
        public event Action<bool> OnUseDebugModeToggle;
        public event Action OnShowAdsDebugButtonPressed;
        public event Action OnClearProgressButtonPressed;

        private const string LastSavedStateKey = "last_saved_state_key";

        [Inject]
        private void Construct(MainMenuScreenPresenterFactory presenterFactory)
        {
            Presenter = presenterFactory.Create(this);
            InitDropdown();
            _timescaleInputField.text = "1";
        }

        private void InitDropdown()
        {
            var stateArray = Enum.GetNames(typeof(GameplayStateMachineConditionManager.StateConfig)).ToList();
            _dropdownSelectStateConfig.ClearOptions();
            _dropdownSelectStateConfig.AddOptions(stateArray);
            _dropdownSelectStateConfig.RefreshShownValue();
            _dropdownSelectStateConfig.value = PlayerPrefs.GetInt(LastSavedStateKey, 0);
        }

        private void SaveLastSelectedState(int selectedState)
        {
            PlayerPrefs.SetInt(LastSavedStateKey, selectedState);
        }

        protected override void SubscribeOnEvents()
        {
            _startGameplayButton.onClick.AddListener(() => OnStartGameplayButtonPressed?.Invoke());
            _startGameplayStateConfigButton.onClick.AddListener(() => OnStartGameplayStateConfigButtonPressed?.Invoke());
            _useDebugModeTogle.onValueChanged.AddListener(value => OnUseDebugModeToggle?.Invoke(value));
            _showAdsDebugButton.onClick.AddListener(() => OnShowAdsDebugButtonPressed?.Invoke());
            _clearProgressButton.onClick.AddListener(() => OnClearProgressButtonPressed?.Invoke());
            _dropdownSelectStateConfig.onValueChanged.AddListener(SaveLastSelectedState);
        }

        protected override void UnsubscribeOnEvents()
        {
            _startGameplayButton.onClick.RemoveAllListeners();
            _startGameplayStateConfigButton.onClick.RemoveAllListeners();
            _dropdownSelectStateConfig.onValueChanged.RemoveAllListeners();
            _useDebugModeTogle.onValueChanged.RemoveAllListeners();
            _showAdsDebugButton.onClick.RemoveAllListeners();
            _clearProgressButton.onClick.RemoveAllListeners();
        }

        public void ActiveScreen(bool active)
        {
            _screenCanvas.enabled = active;
        }

        public int GetDebugTimescale()
        {
            string timescaleInputFieldText = _timescaleInputField.text;
            if (int.TryParse(timescaleInputFieldText, out var timescale))
            {
                return timescale;
            }
            return 1;
        }

        public GameplayStateMachineConditionManager.StateConfig GetCurrentStateConfig()
        {
            return (GameplayStateMachineConditionManager.StateConfig)_dropdownSelectStateConfig.value;
        }
    }
}






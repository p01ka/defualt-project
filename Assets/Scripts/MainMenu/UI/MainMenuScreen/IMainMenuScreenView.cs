using System;
using IdxZero.Gameplay.States;

namespace IdxZero.MainMenu.UI.MainMenuScreen
{
    public interface IMainMenuScreenView
    {
        event Action OnStartGameplayButtonPressed;
        event Action OnStartGameplayStateConfigButtonPressed;
        event Action<bool> OnUseDebugModeToggle;
        event Action OnShowAdsDebugButtonPressed;
        event Action OnClearProgressButtonPressed;

        void ActiveScreen(bool active);

        int GetDebugTimescale();
        GameplayStateMachineConditionManager.StateConfig GetCurrentStateConfig();
    }

}






using System;

namespace IdxZero.Gameplay.UI.GameplayButtonsPanel
{
    public interface IGameplayButtonsPanelView
    {
        event Action OnBackButtonPressed;
        event Action OnSettingsButtonPressed;
    }
}






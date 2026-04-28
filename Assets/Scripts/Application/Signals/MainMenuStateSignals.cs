// ReSharper disable InconsistentNaming
using System;

namespace IdxZero.Application.Signals
{
    public class MainMenuStateSignals
    {
        public class OnShowMainMenu
        {
            public Action MainMenuShowedCallback { get; private set; }

            public OnShowMainMenu(Action mainMenuShowedCallback)
            {
                MainMenuShowedCallback = mainMenuShowedCallback;
            }

            public void AddMenuShowedCallback(Action action)
            {
                MainMenuShowedCallback += action;
            }
        }

        public class OnHideMainMenu
        {
        }

        public class OnActiveMainMenuInteraction
        {
            public readonly bool IsInteractionActive;

            public OnActiveMainMenuInteraction(bool isInteractionActive)
            {
                IsInteractionActive = isInteractionActive;
            }
        }
    }
}
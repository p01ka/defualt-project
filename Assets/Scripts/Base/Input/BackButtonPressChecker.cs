using UnityEngine;

namespace IdxZero.Base.Input
{
    public abstract class BackButtonPressChecker
    {
        protected bool IsCheckAvailable;

        public void Tick()
        {
            if (!IsCheckAvailable) return;
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                BackButtonPressed();
            }
        }

        protected abstract void BackButtonPressed();
    }
}
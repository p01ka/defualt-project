using UnityEngine;

namespace IdxZero.Services.ScreenSession
{
    public class ScreenSessionSetter : IScreenSessionSetter
    {
        public void SetSessionScreen()
        {
            UnityEngine.Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            SetUniRate();
        }

        private void SetUniRate()
        {
            var rateManager = UniRate.RateManager.Instance;
            UniRate.RateManager.Instance.UpdateRate.Mode = UniRate.UpdateRateMode.ApplicationTargetFrameRate;
            UniRate.RateManager.Instance.UpdateRate.Minimum = 60;
            UniRate.RateManager.Instance.FixedUpdateRate.Minimum = 50;
            UniRate.RateManager.Instance.RenderInterval.Maximum = 1;
            UniRate.Debug.RateDebug.DisplayOnScreenData = false;
        }
    }

    public interface IScreenSessionSetter
    {
        void SetSessionScreen();
    }
}
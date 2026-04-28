using System;
using IdxZero.Base.States;
using Zenject;

namespace IdxZero.Gameplay.States
{
    public class State_B_Debug : IBaseState
    {
        public void OnEnter(Action releasePreviousStateCallback)
        {
            UnityEngine.Debug.Log("STATE_B_PRE_DEBUG");
        }

        public void OnExit()
        {
        }

        public void OnRelease()
        {
        }

        #region Factory
        public class Factory : PlaceholderFactory<IBaseState>
        {
        }

        #endregion Factory
    }
}
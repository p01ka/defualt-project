using System;
using IdxZero.Base.States;
using Zenject;

namespace IdxZero.Gameplay.States
{
    public class CameraControlDebugState : IBaseState
    {
        public void OnEnter(Action releasePreviousStateCallback)
        {
            UnityEngine.Debug.Log("CAMERA DEBUG");
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
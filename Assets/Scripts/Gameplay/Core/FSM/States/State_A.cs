using System;
using IdxZero.Base.States;
using Zenject;

namespace IdxZero.Gameplay.States
{
    public class State_A : IBaseState
    {

        public void OnEnter(Action releasePreviousStateCallback)
        {
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
using System;
using UnityEngine;

namespace IdxZero.Base.States
{
    public class StateController
    {
        protected IBaseState CurrentState;

        protected void SetState(IBaseState state)
        {
            CurrentState?.OnExit();
            Action releaseCallback;
            if (CurrentState != null)
            {
                releaseCallback = CurrentState.OnRelease;
            }
            else
            {
                void emptyReleaseCallback()
                {
                    Debug.Log("Empty Current State");
                }
                releaseCallback = emptyReleaseCallback;
            }

            state?.OnEnter(releaseCallback);

            CurrentState = state;
        }
    }
}
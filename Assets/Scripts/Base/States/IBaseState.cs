// ReSharper disable InconsistentNaming
using System;

namespace IdxZero.Base.States
{
    public interface IBaseState
    {
        void OnEnter(Action releasePreviousStateCallback);

        void OnExit();

        void OnRelease();
    }
}
using System;

namespace IdxZero.Services.Firebase
{
    public interface IFirebaseService
    {
        void InitializeService(Action serviceInitialized);
    }
}
using System;

namespace IdxZero.Application.Model.UserStatus
{
    public interface IUserStatusGetter
    {
        bool IsNoAdsUser { get; }

        event Action<bool> OnUserStatusChanged;
    }
}
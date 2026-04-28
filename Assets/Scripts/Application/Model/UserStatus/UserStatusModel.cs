using System;

namespace IdxZero.Application.Model.UserStatus
{
    public class UserStatusModel : IUserStatusGetter, IUserStatusSetter
    {
        public bool IsNoAdsUser
        {
            get => _isNoAdsUser;
            set
            {
                _isNoAdsUser = value;
                OnUserStatusChanged?.Invoke(value);
            }
        }

        public event Action<bool> OnUserStatusChanged;

        private bool _isNoAdsUser;

    }
}
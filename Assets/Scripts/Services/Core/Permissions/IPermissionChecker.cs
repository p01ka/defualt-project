using System;

namespace IdxZero.Services.Permissions
{
    public interface IPermissionChecker
    {
        void CheckCameraPermission(Action successCallback, Action errorCallback);
        void CheckGalleryPermission(Action<bool> requestPermissionCallback);
        void CheckNotificationPermission(Action succeedCallback, Action errorCallback);
    }
}
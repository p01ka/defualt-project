using IdxZero.Utils;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace IdxZero.Services.Permissions
{
    public class UserPermissionChecker : IPermissionChecker
    {
        private const string NoCameraPermissionMessage =
            "Application does not have access to the camera, please change the privacy settings";

        private const string NoGalleryPermissionMessage =
            "Application does not have access to the gallery, please change the privacy settings";

        public void CheckCameraPermission(Action successCallback, Action errorCallback = null)
        {
#if UNITY_ANDROID
            CheckAndroidPermission(successCallback, errorCallback);
#elif UNITY_IOS
            CheckIOSPermission(successCallback, errorCallback);
#endif
        }

        public void CheckGalleryPermission(Action<bool> requestPermissionCallback)
        {
            var permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write);
            switch (permission)
            {
                case NativeGallery.Permission.Granted:
                    requestPermissionCallback?.Invoke(true);
                    break;

                case NativeGallery.Permission.ShouldAsk:
                    var requestedPermission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Write);
                    requestPermissionCallback?.Invoke(requestedPermission == NativeGallery.Permission.Granted);
                    break;

                case NativeGallery.Permission.Denied:

                    void GalleryPermissionCallback(bool gettedPermission)
                    {
                        requestPermissionCallback?.Invoke(gettedPermission);
                    }

                    CheckGalleryDeniedPermission(GalleryPermissionCallback);
                    break;
            }
        }

        public void CheckNotificationPermission(Action succeedCallback, Action errorCallback)
        {
#if UNITY_IOS
            Timing.RunCoroutine(RequestAuthorization(succeedCallback, errorCallback));
#else
            succeedCallback?.Invoke();
#endif
        }

        private void CheckGalleryDeniedPermission(Action<bool> permissionCallback)
        {
#if UNITY_ANDROID
            var requestedPermission = NativeGallery.RequestPermission(NativeGallery.PermissionType.Write);
            permissionCallback?.Invoke(requestedPermission == NativeGallery.Permission.Granted);
#elif UNITY_IOS
            permissionCallback?.Invoke(false);
            TryOpenSettingsDialog(NoGalleryPermissionMessage);
#endif
        }

        private void CheckAndroidPermission(Action successCallback, Action errorCallback = null)
        {
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            {
                Timing.RunCoroutine(WaitAndroidRequest(successCallback));
            }
            else
            {
                successCallback?.Invoke();
            }
        }

        private IEnumerator<float> WaitAndroidRequest(Action callback)
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
            while (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            {
                yield return Timing.WaitForSeconds(1f);
            }

            callback?.Invoke();
        }

        private void CheckIOSPermission(Action successCallback, Action errorCallback = null)
        {
            Debug.Log("IOS CAMERA AUTHORIZED " +
                      UnityEngine.Application.HasUserAuthorization(UserAuthorization.WebCam));

            if (!UnityEngine.Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                Timing.RunCoroutine(WaitIOSRequest(successCallback, errorCallback));
            }
            else
            {
                successCallback?.Invoke();
            }
        }

        private IEnumerator<float> WaitIOSRequest(Action callback, Action errorCallback)
        {
            yield return Timing.WaitUntilDone(
                UnityEngine.Application.RequestUserAuthorization(UserAuthorization.WebCam));
            if (UnityEngine.Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                callback?.Invoke();
            }
            else
            {
                TryOpenSettingsDialog(NoCameraPermissionMessage);
                errorCallback?.Invoke();
            }
        }

        private void TryOpenSettingsDialog(string message)
        {
            if (NativeGallery.CanOpenSettings())
            {
                MobileNativeDialog.NativeDialog.OpenDialog(BuildConsts.PRODUCT_NAME, message, "Ok", "Open settings",
                    () => { Debug.Log("Ok Button pressed"); },
                    NativeGallery.OpenSettings);
            }
        }

#if UNITY_IOS
        private IEnumerator<float> RequestAuthorization(Action successCallback = null, Action errorCallbak = null)
        {
            using (var req =
 new Unity.Notifications.iOS.AuthorizationRequest(Unity.Notifications.iOS.AuthorizationOption.Alert | Unity.Notifications.iOS.AuthorizationOption.Badge, true))
            {
                while (!req.IsFinished)
                {
                    yield return 0;
                };

                if (req.Granted)
                    successCallback?.Invoke();
                else
                {
                    errorCallbak?.Invoke();
                }
            }
        }
#endif
    }
}
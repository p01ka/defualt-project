// ReSharper disable InconsistentNaming

using UnityEngine;

namespace IdxZero.Utils
{
    public class Consts
    {
        public const string FIRST_RUN_JSON_NAME = "firstRunJson";
        public const int WEB_REQUEST_TIME_OUT = 15;
        public const int MIN_AVAILABLE_DISKSPACE_TO_CHECK_MB = 100;

        public static readonly float DEFAULT_SCREEN_WIDTH;

        public static readonly float DEFAULT_X_POSITION_CONTENT_SCREEN;
        public static readonly float DISABLED_X_POSITION_CONTENT_SCREEN;

        public const float ACTIVE_X_POSITION_CONTENT_SCREEN = 0;
        public const float TRANSITION_TIME_CONTENT_SCREEN = 0.25f;
        public const LeanTweenType ScreenTransitionTweenType = LeanTweenType.linear;

        public const string DEVICE_PREVIEWS_ANDROID_PATH = "androidPreviews/previewimages";
        public const string DEVICE_PREVIEWS_IOS_PATH = "iOSPreviews/previewimages";

        public const string DEVICE_PRELOADED_ANDROID_BUNDLES_FOLDER = "androidBundle";
        public const string DEVICE_PRELOADED_IOS_BUNDLES_FOLDER = "iOSBundle";

        public const float GAMEPLAY_CAMERA_EDGE_OFFSET = 0.95f;

        public const int VISIBLE_ITEMS_COUNT_PHONE = 7;
        public const int VISIBLE_ITEMS_COUNT_TABLET = 9;

        public const int MAX_MESH_CALCULATION_POINTS_COUNT = 30;

        public const int SAVED_ITEM_NAME_MAX_LENGTH = 50;
        public const int MAX_VISIBLE_ITEM_NAME_CHARS_COUNT = 23;

        public const float HIDE_SPLASH_SCREEN_OFFSET = 0.5f;

        static Consts()
        {
            var aspect = (float) Screen.width / Screen.height;
            DEFAULT_SCREEN_WIDTH = aspect <= 720f / 1280f
                ? 720
                : 1280 * aspect;

            DEFAULT_X_POSITION_CONTENT_SCREEN = DEFAULT_SCREEN_WIDTH;
            DISABLED_X_POSITION_CONTENT_SCREEN = -DEFAULT_SCREEN_WIDTH;
        }
        
        public const string TermsOfUseUrl = "http://exomind.gg/termsofuse.txt";
        public const string PrivacyPolicyUrl = "http://exomind.gg/privacypolicy.txt";
    }
}
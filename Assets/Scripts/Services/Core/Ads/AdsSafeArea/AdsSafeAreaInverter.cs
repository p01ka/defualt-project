using Crystal;
using UnityEngine;

namespace IdxZero.Services.Ads
{
    public class AdsSafeAreaInverter : SafeAreaBase
    {
        // [SerializeField] 
        [Range(-1f, 1f)] private float _offsetAnchorMinY = -0.005f;
        // [SerializeField] 
        [Range(-1f, 1f)] private float _offsetAnchorMaxY = 0.005f;

        protected override void ApplySafeArea(Rect r)
        {

            LastSafeArea = r;

            // Ignore x-axis?
            if (!ConformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                Vector2 anchorMin = r.position;
                Vector2 anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                float xOffset = anchorMin.x + (1 - anchorMax.x);
                float yOffset = anchorMin.y + (1 - anchorMax.y);

                anchorMin = new Vector2(-(anchorMin.x / (1 - xOffset)), -(anchorMin.y / (1 - yOffset)) + _offsetAnchorMinY);
                anchorMax = new Vector2(1 + (1 - anchorMax.x) / (1 - xOffset), 1 + (1 - anchorMax.y) / (1 - yOffset) + _offsetAnchorMaxY);

                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                // See https://forum.unity.com/threads/569236/page-2#post-6199352
                if (/*anchorMin.x >= 0 && anchorMin.y >= 0 &&*/ anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    Panel.anchorMin = anchorMin;
                    Panel.anchorMax = anchorMax;
                }
            }

            if (Logging)
            {
                Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                    name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
            }
        }
    }
}
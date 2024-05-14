using UnityEngine;

namespace Game
{
    public enum EPlaceOrientation
    {
        VERTICAL,
        HORIZONTAL,
    }

    public static class RTPlaceUtil
    {
        public static void SetVerticalMin(RectTransform rt, float value)
        {
            Vector2 anchor = rt.anchorMin;
            anchor.y = value;
            rt.anchorMin = anchor;
        }

        public static void SetVerticalMax(RectTransform rt, float value)
        {
            Vector2 anchor = rt.anchorMax;
            anchor.y = value;
            rt.anchorMax = anchor;
        }
        public static void SetVerticalMinMax(RectTransform rt, float min, float max)
        {
            float x = rt.sizeDelta.x;
            SetVerticalMin(rt, min);
            SetVerticalMax(rt, max);
            Vector2 sizeDelta = rt.sizeDelta;
            sizeDelta.x = x;
            // rt.sizeDelta = sizeDelta;
        }

        public static void SetHorizontalMin(RectTransform rt, float value)
        {
            Vector2 anchor = rt.anchorMin;
            anchor.x = value;
            rt.anchorMin = anchor;
        }

        public static void SetHorizontalMax(RectTransform rt, float value)
        {
            Vector2 anchor = rt.anchorMax;
            anchor.x = value;
            rt.anchorMax = anchor;
        }

        public static void SetHorizontalMinMax(RectTransform rt, float min, float max)
        {
            float y = rt.sizeDelta.y;
            SetHorizontalMin(rt, min);
            SetHorizontalMax(rt, max);
            Vector2 sizeDelta = rt.sizeDelta;
            sizeDelta.y = y;
            rt.sizeDelta = sizeDelta;
        }
    }
}
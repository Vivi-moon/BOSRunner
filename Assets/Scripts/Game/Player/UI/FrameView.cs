using UnityEngine;
using UnityEngine.UI;
using Common;

namespace Game.Player
{
    public class FrameView : DeviceViewBase
    {
        [Space]
        [SerializeField] private Image _frameFill;


        /// <param name="min">0..100</param>
        /// <param name="max">0..100</param>
        public void SetData(int min, int max)
        {
            RectTransform rt = _frameFill.rectTransform;
            float minF = GetPercentF(min);
            float maxF = GetPercentF(max);
            switch (Orientation)
            {
                case EPlaceOrientation.HORIZONTAL:
                    RTPlaceUtil.SetHorizontalMinMax(rt, minF, maxF);
                    break;
                case EPlaceOrientation.VERTICAL:
                default:
                    RTPlaceUtil.SetVerticalMinMax(rt, minF, maxF);
                    break;
            }
        }

        public override void SetColor(Color value)
        {
            _frameFill.color = value;
        }
    }
}
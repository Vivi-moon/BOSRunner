using UnityEngine;
using UnityEngine.UI;
using Common;

namespace Game.Player
{
    public class ColumnView : DeviceViewBase
    {
        [Space]
        [SerializeField] private Image _columnFill;


        /// <param name="value">0..100</param>
        public void SetData(int value)
        {
            RectTransform rt = _columnFill.rectTransform;
            float valueF = GetPercentF(value);
            switch (Orientation)
            {
                case EPlaceOrientation.HORIZONTAL:
                    RTPlaceUtil.SetHorizontalMax(rt, valueF);
                    break;
                case EPlaceOrientation.VERTICAL:
                default:
                    RTPlaceUtil.SetVerticalMax(rt, valueF);
                    break;
            }
        }

        public override void SetColor(Color value)
        {
            _columnFill.color = value;
        }
    }
}
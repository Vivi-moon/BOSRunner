using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfoValuesView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _frameIndicator;
        [SerializeField] private Text _dataInfo;
        [Space]
        [SerializeField] private string _dataInfoFormat = "c: {0}, frame: {1}/{2}";
        [SerializeField] private Color _normalFrameIndicatorColor = Color.green;
        [SerializeField] private Color _badFrameIndicatorColor = Color.yellow;

        
        public void SetDataInfo(int v, int fBottom, int fTop)
        {
            _dataInfo.text = string.Format(_dataInfoFormat, v, fBottom, fTop);
        }

        public void SetFrameStatus(bool isNormal)
        {
            _frameIndicator.color = isNormal
                ? _normalFrameIndicatorColor
                : _badFrameIndicatorColor;
        }
    }
}
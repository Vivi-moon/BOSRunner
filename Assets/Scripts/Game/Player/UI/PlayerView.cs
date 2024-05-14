using Common;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayerView : MonoBehaviour
    {
        public bool InfoEnabled => _infoEnabled;

        [SerializeField] private PlayerInfoView _playerInfoView;
        [SerializeField] private ValuesView _valuesView;
        [SerializeField] private PlayerInfoValuesView _playerValuesInfoView;

        private bool _infoEnabled;

        private RectTransform _rt;


        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _infoEnabled = _playerInfoView.gameObject.activeSelf;
        }

        public void SetName(string name)
        {
            _playerInfoView.SetName(name);
        }

        public void UpdateData(int column, int frameBottom, int frameTop, EFrameZoneType frameZone)
        {
            _valuesView.UpdateData(column, frameBottom, frameTop, frameZone);
            _playerValuesInfoView.SetDataInfo(column, frameBottom, frameTop);
        }

        public void UpdateFrameStatus(bool isNormal)
        {
            _playerValuesInfoView.SetFrameStatus(isNormal);
        }

        public void SetViewAnchors(EPlaceOrientation orientation, float value)
        {
            switch (orientation)
            {
                case EPlaceOrientation.HORIZONTAL:
                    RTPlaceUtil.SetHorizontalMinMax(_rt, value, value);
                    break;
                case EPlaceOrientation.VERTICAL:
                default:
                    RTPlaceUtil.SetVerticalMinMax(_rt, value, value);
                    break;
            }
        }
    }
}
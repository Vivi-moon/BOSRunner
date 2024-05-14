using System;
using UnityEngine;
using Common;

namespace Game.Player
{
    [RequireComponent(typeof(RectTransform))]
    public class ValuesView : MonoBehaviour
    {
        public enum EFrameZoneSelect
        {
            NONE,
            FRAME,
            COLUMN,
        }

        [System.Serializable]
        public struct FrameZoneColorItem
        {
            public EFrameZoneType zone;
            public Color color;

            public FrameZoneColorItem(EFrameZoneType zone, Color color)
            {
                this.zone = zone;
                this.color = color;
            }
        }

        [Space]
        [SerializeField] private ColumnView _columnView;
        [SerializeField] private FrameView _frameView;
        [Space]
        [SerializeField] private EFrameZoneSelect _frameZoneChangeTarget = EFrameZoneSelect.NONE;
        [SerializeField] private FrameZoneColorItem[] _frameColorPerZone = GetDefaultColors();

        public void UpdateData(int column, int frameBottom, int frameTop, EFrameZoneType frameZone)
        {
            _columnView.SetData(column);
            _frameView.SetData(frameBottom, frameTop);

            var colorTarget = GetColorTarget();
            if (colorTarget != null)
            {
                colorTarget.SetColor(GetColorFor(frameZone));
            }
        }

        private DeviceViewBase GetColorTarget()
        {
            switch (_frameZoneChangeTarget)
            {
                case EFrameZoneSelect.COLUMN:
                    return _columnView;
                case EFrameZoneSelect.FRAME:
                    return _frameView;
                default:
                    return null;
            }
        }

        private Color GetColorFor(EFrameZoneType frameZone)
        {
            if (_frameColorPerZone == null) { return Color.white; }

            for (int i = 0; i < _frameColorPerZone.Length; i++)
            {
                if (frameZone == _frameColorPerZone[i].zone)
                {
                    return _frameColorPerZone[i].color;
                }
            }

            return Color.white;
        }

        private void OnValidate()
        {
            if (_frameColorPerZone != null)
            {
                var zones = (EFrameZoneType[])Enum.GetValues(typeof(EFrameZoneType));

                for (int i = 0; i < zones.Length; i++)
                {
                    if (!Array.Exists(_frameColorPerZone, itm => itm.zone == zones[i]))
                    {
                        Debug.LogErrorFormat("[FrameView] {0}.{1} not found in {2}"
                            , nameof(EFrameZoneType), zones[i], nameof(_frameColorPerZone));
                        ResetColorZones();
                        return;
                    }
                }
            }
            else
            {
                ResetColorZones();
            }
        }

        [ContextMenu("Reset color zones")]
        private void ResetColorZones()
        {
            _frameColorPerZone = GetDefaultColors();
        }

        private static FrameZoneColorItem[] GetDefaultColors()
        {
            Color redZone = new Color(188/255f, 67/255f, 85/255f);
            Color orangeZone = new Color(247/255f, 216/255f, 106/255f);
            Color greenZone = new Color(131/255f, 247/255f, 219/255f);
            return new FrameZoneColorItem[]
            {
                new FrameZoneColorItem(EFrameZoneType.NULL, redZone),
                new FrameZoneColorItem(EFrameZoneType.RED_DOWN, redZone),
                new FrameZoneColorItem(EFrameZoneType.ORANGE_DOWN, orangeZone),
                new FrameZoneColorItem(EFrameZoneType.GREEN, greenZone),
                new FrameZoneColorItem(EFrameZoneType.ORANGE_UP, orangeZone),
                new FrameZoneColorItem(EFrameZoneType.RED_UP, redZone),
            };
        }
    }
}
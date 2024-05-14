using UnityEngine;
using Common;

namespace Game.Player
{
    public abstract class DeviceViewBase : MonoBehaviour
    {
        public EPlaceOrientation Orientation => _orientation;

        [SerializeField] private EPlaceOrientation _orientation = EPlaceOrientation.VERTICAL;


        public abstract void SetColor(Color value);

        protected float GetPercentF(int percent) => percent / 100.0f;
    }
}
using Common;
using Common.BFB;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController
    {
        private PlayerView _view;
        private int _id => _device.Id;
        private IDevice _device;
        private Transform _pos_hero;


        public void Init(PlayerView view, IDevice device)
        {
            _view = view;
            _device = device;

            _view.SetName(_id.ToString());
        }

        

        public void Deinit()
        {

        }

        public int UpdateData(double gameTime)
        {
            int value = _device.ReadValue();
            int fBottom = 0;
            int fTop = 100;
            // int fBottom = _device.ReadBottom(gameTime);
            // int fTop = _device.ReadTop(gameTime);
            EFrameZoneType fZoneType = _device.ReadColorGroup();
            EFrameStatus fStatus = _device.ReadFrame(gameTime, out fBottom, out fTop);

            _view.UpdateData(value, fBottom, fTop, fZoneType);
            _view.UpdateFrameStatus(fStatus != EFrameStatus.WAIT_PERIOD);
            return value;
        }

        /*private int UpdateDataHero(double gameTime)
        {
            int value = _device.ReadValue();
            return value == 0 ? 0 : value;
        }*/

    }
}
using Biofeedback;
using Common.Game;

namespace Common.BFB
{
    public sealed class DeviceProvider : BfbProviderBase, IDevice
    {
        public int Id => _chId;

        private int _chId;


        public DeviceProvider(IDataProviderBFB bfb, int chId) : base(bfb)
        {
            _chId = chId;
        }

        public override void Deinit()
        {
            
        }

        public int ReadValue() => bfb.ReadValue(_chId);
        public float ReadValueF() => bfb.ReadValueF(_chId);

        public EFrameZoneType ReadColorGroup() => (EFrameZoneType)bfb.ReadColorGroup(_chId);

        public EFrameStatus ReadFrame(double forTimeSec, out int bottom, out int top)
            => (EFrameStatus)bfb.ReadFrame(forTimeSec, out bottom, out top, _chId);

        public int ReadBottom(double forTimeSec) => bfb.ReadBottom(forTimeSec, _chId);
        public int ReadTop(double forTimeSec) => bfb.ReadTop(forTimeSec, _chId);
    }
}
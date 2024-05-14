
namespace Common.BFB
{
    public interface IDevice
    {
        /// <summary>
        /// device channel
        /// </summary>
        int Id { get; }

        /// <summary>
        /// 0..100
        /// </summary>
        int ReadValue();
        /// <summary>
        /// 0..100
        /// </summary>
        float ReadValueF();

        EFrameZoneType ReadColorGroup();

        /// <param name="forTimeSec">current or future (if EFrameStatus.FULL_PERIOD) game time</param>
        EFrameStatus ReadFrame(double forTimeSec, out int bottom, out int top);

        /// <param name="forTimeSec">current or future (if EFrameStatus.FULL_PERIOD) game time</param>
        int ReadBottom(double forTimeSec);
        /// <param name="forTimeSec">current or future (if EFrameStatus.FULL_PERIOD) game time</param>
        int ReadTop(double forTimeSec);
    }
}
namespace Common.BFB
{
    public interface IBFBGameProvider
    {
        bool IsInitialized { get; }

        IGameConnection GameConnection { get; }

        int DevicesCount { get; }
        /// <param name="id">0..DevicesCount-1</param>
        IDevice GetDeviceFor(int id);

        double CurrentGameTime { get; }
        double CurrentUnusedTime { get; }
    }
}
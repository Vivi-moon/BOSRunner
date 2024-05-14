namespace Common.BFB
{
    /// <summary>
    /// NO_PERIOD - frame is static and changes manually (cannot be predicted).
    /// WAIT_PERIOD of FULL_PERIOD - the frame changes according to a preset rule.
    /// FULL_PERIOD - it is possible to predict the frame, i.e. to get +x sec into the future.
    /// </summary>
    public enum EFrameStatus
    {
        NO_PERIOD = -1,
        WAIT_PERIOD = 0,
        FULL_PERIOD = 1,
    }
}
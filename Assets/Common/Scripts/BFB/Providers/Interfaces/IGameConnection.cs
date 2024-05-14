using System;
using Biofeedback;

namespace Common.BFB
{
    public interface IGameConnection
    {
        event Action<EGameConnectionState> OnStateChanged;
        bool IsConnected { get; }
        ErrorGame LastError { get; }
        EGameConnectionState LastState { get; }
    }
}
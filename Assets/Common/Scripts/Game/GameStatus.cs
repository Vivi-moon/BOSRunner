using Common.BFB;
using System;

namespace Common.Game
{
    public class GameStatus
    {
        public event Action<bool> GamePauseChanged;

        public bool IsPaused => ParseConnectionState(_bfb.GameConnection.LastState);

        private IBFBGameProvider _bfb;


        private GameStatus() { }
        public GameStatus(IBFBGameProvider bfb)
        {
            _bfb = bfb;
            AddListeners();

            OnGameConnectionChanged(_bfb.GameConnection.LastState);
        }

        public void Deinit()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _bfb.GameConnection.OnStateChanged += OnGameConnectionChanged;
        }
        private void RemoveListeners()
        {
            _bfb.GameConnection.OnStateChanged -= OnGameConnectionChanged;
        }

        private bool ParseConnectionState(EGameConnectionState state)
            => state != EGameConnectionState.STARTED;

        private void OnGameConnectionChanged(EGameConnectionState state)
        {
            GamePauseChanged?.Invoke(ParseConnectionState(state));
        }
    }
}
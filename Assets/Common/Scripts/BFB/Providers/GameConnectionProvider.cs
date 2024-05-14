using System;
using Biofeedback;

namespace Common.BFB
{
    public sealed class GameConnectionProvider : BfbProviderBase, IGameConnection
    {
        public event Action<EGameConnectionState> OnStateChanged;

        public bool IsConnected => bfb.IsConnected;

        public ErrorGame LastError => bfb.LastErrorGame;
        public EGameConnectionState LastState => (EGameConnectionState)bfb.GameState;

        private BFBGameProvider _bfbGame;


        public GameConnectionProvider(BFBGameProvider bfbGame, IDataProviderBFB bfb) : base(bfb)
        {
            _bfbGame = bfbGame;
            AddListeners();
        }

        public override void Deinit()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            bfb.OnDisconnected += OnDisconnectedReceived;
            bfb.StateChanged += OnConnectionStateChanged;
            bfb.ErrorOccurred += OnErrorReceived;
        }
        private void RemoveListeners()
        {
            bfb.OnDisconnected -= OnDisconnectedReceived;
            bfb.StateChanged -= OnConnectionStateChanged;
            bfb.ErrorOccurred -= OnErrorReceived;
        }

        private void OnDisconnectedReceived()
        {
            Logger.Log(nameof(GameConnectionProvider), "received server disconnect");
        }

        private void OnConnectionStateChanged(object sender, GameState state)
        {
            switch (state)
            {
                case GameState.STOPPED:
                    break;
                case GameState.NOT_READY:
                    break;
                case GameState.STARTED:
                    break;
                case GameState.SERVER_PAUSED:
                    break;
                default:
                    break;
            }

            Logger.Log(nameof(GameConnectionProvider), "received server state: " + state);
            _bfbGame.CallInUpdate(() =>
                OnStateChanged?.Invoke((EGameConnectionState)state));
        }

        private void OnErrorReceived(object sender, ErrorGame err)
        {
            Logger.LogException(
                  nameof(GameConnectionProvider)
                , string.Format("err type: {0}; msg: {1})", err.type, err.desc)
                , err.exception
            );
        }
    }
}
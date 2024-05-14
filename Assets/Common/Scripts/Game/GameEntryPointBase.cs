using UnityEngine;
using Common.BFB;

namespace Common.Game
{
    public abstract class GameEntryPointBase : MonoBehaviour
    {
        public GameStatus GameStatus => _gameStatus;

        public int PlayersCount => _bfb.DevicesCount;
        public double GameTime => _bfb.CurrentGameTime;

        private IBFBGameProvider _bfb;
        private GameStatus _gameStatus;


        private void Start()
        {
            CheckOtherEntries();

            _bfb = GameUtils.FindBFBGameProvider();

            if (_bfb == null || !_bfb.IsInitialized)
            {
                Debug.LogError("BFB not ready!");
            }

            _gameStatus = new GameStatus(_bfb);

            Initialize();
        }

        private void OnDestroy()
        {
            DeInitialize();
            _gameStatus.Deinit();
        }

        public IDevice GetDevice(int id) => _bfb.GetDeviceFor(id);

        protected abstract void Initialize();
        protected abstract void DeInitialize();

        private void CheckOtherEntries()
        {
            var entries = FindObjectsOfType<GameEntryPointBase>();

            if (entries.Length > 1)
            {
                Debug.LogErrorFormat("Founded more than 1 {0}, delete duplicates", nameof(GameEntryPointBase));
            }
        }
    }
}
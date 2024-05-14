using UnityEngine;

namespace Game
{
    public abstract class GameControllerBase : MonoBehaviour, IGameController
    {
        public bool IsInitialized => _isInitialized;
        public GameEntryPoint gameEntry => _gameEntry;

        private GameEntryPoint _gameEntry;
        private bool _isInitialized;


        public void Initialize(GameEntryPoint gameEntry)
        {
            _gameEntry = gameEntry;
            Init();
            _isInitialized = true;
        }

        public void DeInitialize()
        {
            _isInitialized = false;
            DeInit();
        }

        protected abstract void Init();
        protected abstract void DeInit();
    }
}
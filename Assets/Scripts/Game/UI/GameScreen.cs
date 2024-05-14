using UnityEngine;
using Common.BFB;
using Common.Game;

namespace Game
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private GameOverlay _pauseOverlay;

        private GameEntryPoint _gameEntry;

        public void Init(GameEntryPoint gameEntry)
        {
            _gameEntry = gameEntry;
            AddListeners();
            OnPauseChanged(gameEntry.GameStatus.IsPaused);
        }

        public void DeInit()
        {
            RemoveListeners();
        }

        public void SetPause(bool pauseEnabled)
        {
            _pauseOverlay?.gameObject.SetActive(pauseEnabled);
        }

        private void AddListeners()
        {
            _gameEntry.GameStatus.GamePauseChanged += OnPauseChanged;
        }
        private void RemoveListeners()
        {
            _gameEntry.GameStatus.GamePauseChanged -= OnPauseChanged;
        }

        private void OnPauseChanged(bool isPaused)
        {
            SetPause(isPaused);
        }
        private void OnConnectionStateChanged(EGameConnectionState state)
        {
            SetPause(state != EGameConnectionState.STARTED);
        }

    }
}

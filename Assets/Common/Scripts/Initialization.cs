using Stopwatch = System.Diagnostics.Stopwatch;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common.BFB;

namespace Common
{
    public class Initialization : MonoBehaviour
    {
        [SerializeField] private float _waitConnectionTimeSec = 5.0f;

        private BFBGameProvider _bfbProvider;
        private IGameConnection _gameConnection;

        private bool BfbNoChannels => _bfbProvider.DevicesCount == 0;
        private bool BfbNotRunning => _gameConnection.LastState != EGameConnectionState.STARTED
                                   && _gameConnection.LastState != EGameConnectionState.PAUSED;


        private void Start()
        {
            _bfbProvider = BFBGameProvider.Create();
            if (_bfbProvider == null) { return; }

            _gameConnection = _bfbProvider.GameConnection;
            StartCoroutine(WaitConnectionRoutine());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator WaitConnectionRoutine()
        {
            long timeoutMs = (long)(_waitConnectionTimeSec * 1000);
            var stopwatch = Stopwatch.StartNew();
            yield return null;

            while (!_gameConnection.IsConnected || BfbNoChannels || BfbNotRunning)
            {
                if (stopwatch.ElapsedMilliseconds > timeoutMs)
                {
                    Debug.Log("Biofeedback connection timeout!");
                    _bfbProvider.SetNeedQuit();
                }

                yield return null;
            }
            stopwatch.Stop();

            WaitConnectionEnd();
        }

        private void WaitConnectionEnd()
        {
            _bfbProvider.Initialize();
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1
                , LoadSceneMode.Single);
        }
    }
}
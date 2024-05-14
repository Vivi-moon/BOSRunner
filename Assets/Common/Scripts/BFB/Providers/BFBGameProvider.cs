using System;
using UnityEngine;
using Biofeedback;
using System.Collections.Generic;

namespace Common.BFB
{
    public sealed class BFBGameProvider : MonoBehaviour, IBFBGameProvider
    {
        public bool IsInitialized => _initialized;

        public IGameConnection GameConnection => _connection;
        public int DevicesCount => _bfb.ChannelsCount;
        public double CurrentGameTime => _timer.GameTime;
        public double CurrentUnusedTime => _timer.UnusedTime;

        private IDataProviderBFB _bfb;
        private GameTimer _timer;

        private GameConnectionProvider _connection;
        private DeviceProvider[] _devices;

        private bool _initialized;

        private bool _needQuit = false;
        private List<Action> _updateActions = new List<Action>();


        public static BFBGameProvider Create()
        {
            var check = FindObjectsOfType<BFBGameProvider>();
            if (check.Length > 0)
            {
                string t = nameof(BFBGameProvider);
                Logger.LogError(t, "delete other " + t);
                return null;
            }

            var go = new GameObject("- " + nameof(BFBGameProvider) + " -");
            DontDestroyOnLoad(go);

            var instance = go.AddComponent<BFBGameProvider>();
            instance.InitBfb();
            instance.InitGameConnection(instance._bfb);

            return instance;
        }

        public void Initialize()
        {
            InitTimer();
            InitDevices(_bfb);
            _timer.GameTimerStart();
            _initialized = true;
        }

        public void DeInitialize()
        {
            _initialized = false;
            _bfb?.Stop();
            _connection?.Deinit();
            if (_devices != null)
            {
                Array.ForEach(_devices, d => d.Deinit());
            }
        }

        public IDevice GetDeviceFor(int id)
        {
            if (_devices == null || _devices.Length < id+1) { return null; }

            return _devices[id];
        }

        internal void CallInUpdate(Action action)
        {
            _updateActions.Add(action);
        }

        internal void SetNeedQuit()
        {
            _needQuit = true;
        }

        private void OnBFBDisconnected()
        {
            _bfb.OnDisconnected -= OnBFBDisconnected;
            SetNeedQuit();
        }

        private void CloseApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Update()
        {
            _timer?.UpdateGameTimer(_connection.IsConnected);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _bfb.Stop();
            }

            if (_needQuit)
            {
                CloseApp();
            }

            InvokeUpdateActions();

            BFBSimulation.UpdateControls(_bfb);
        }

        private void OnDestroy()
        {
            DeInitialize();
        }

        private void InvokeUpdateActions()
        {
            var tmpArr = _updateActions.ToArray();
            _updateActions.Clear();

            Array.ForEach(tmpArr, act => act?.Invoke());
        }

        #region Initialization

        private void InitBfb()
        {
#if !SIMULATE || !DEVELOPMENT
            _bfb = GameFab.InstanceBFB();
#else
            _bfb = BFBSimulation.CreateSimulate();
#endif

            _bfb.OnDisconnected += OnBFBDisconnected;

            _bfb.Start();
        }

        private void InitTimer()
        {
            _timer = new GameTimer();
        }

        private void InitDevices(IDataProviderBFB bfb)
        {
            int chCount = bfb.ChannelsCount;

            Debug.Log("Devices count: " + chCount);
            _devices = new DeviceProvider[chCount];
            for (int i = 0; i < chCount; i++)
            {
                _devices[i] = new DeviceProvider(bfb, i);
            }
        }

        private void InitGameConnection(IDataProviderBFB bfb)
        {
            _connection = new GameConnectionProvider(this, bfb);
            _connection.OnStateChanged += OnGameConnectionStateChanged;
            OnGameConnectionStateChanged(_connection.LastState);
        }

        private void OnGameConnectionStateChanged(EGameConnectionState state)
        {
            if (_connection == null || state != EGameConnectionState.STARTED) { return; }

            _connection.OnStateChanged -= OnGameConnectionStateChanged;
        }

        #endregion
    }
}
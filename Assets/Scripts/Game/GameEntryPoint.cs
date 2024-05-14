using System;
using UnityEngine;
using Common.Game;
using Game.Player;

namespace Game
{
    public class GameEntryPoint : GameEntryPointBase
    {
        public GameScreen GameScreen => _gameScreen;
        public GameHUD GameHUD => _gameHUD;
        public PlayersManager Players => _players;

        [SerializeField] private GameControllerBase[] _gameControllers;
        [Space]
        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private GameHUD _gameHUD;
        [SerializeField] private PlayersManager _players;

        protected override void Initialize()
        {
            _players.Initialize(this);
            _gameScreen?.Init(this);
            _gameHUD?.Init();
            Array.ForEach(_gameControllers, gc => gc.Initialize(this));
        }

        protected override void DeInitialize()
        {
            Array.ForEach(_gameControllers, gc => gc.DeInitialize());
            _gameScreen?.DeInit();
            _gameHUD?.DeInit();
            _players.DeInitialize();
        }
    }
}
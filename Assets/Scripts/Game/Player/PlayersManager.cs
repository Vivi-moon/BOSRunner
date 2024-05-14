using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class PlayersManager : GameControllerBase
    {
        private List<PlayerController> _players = new List<PlayerController>();
        public Transform pos_hero;


        protected override void Init()
        {
            InitPlayers();
        }

        protected override void DeInit()
        {
            _players.ForEach(p => p.Deinit());
        }

        private void InitPlayers()
        {
            int count = gameEntry.PlayersCount;

            for (int i = 0; i < count; i++)
            {
                CreatePlayer(i);
            }
        }

        private void CreatePlayer(int id)
        {
            var player = new PlayerController();
            player.Init(
                  gameEntry.GameHUD.CreatePlayerView(id)
                , gameEntry.GetDevice(id)
            );

            _players.Add(player);
        }

        

        private void Update()
        {
            if (!IsInitialized) { return; }


            //pos_hero.position = new Vector3(10, 0, 0);

            for (int i = 0; i < _players.Count; i++)
            {
                int value = _players[i].UpdateData(gameEntry.GameTime);
                pos_hero.position = new Vector3((value/6 - 8), 0, 0);
            }
        }
    }
}
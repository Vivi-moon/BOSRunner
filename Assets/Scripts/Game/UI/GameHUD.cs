using System.Collections.Generic;
using Common;
using Game.Player;
using UnityEngine;

namespace Game
{
    public class GameHUD : MonoBehaviour
    {
        [SerializeField] private EPlaceOrientation _playerViewsOrientation = EPlaceOrientation.HORIZONTAL;
        [SerializeField] private GameObject _playerViewsContainer;
        [Space]
        [SerializeField] private bool _enablePlayerInfo = true;
        [Space]
        [SerializeField] private PlayerView _playerViewPrefab;

        private List<PlayerView> _playerViews = new List<PlayerView>();


        public void Init()
        {
            AddListeners();
        }

        public void DeInit()
        {
            RemoveListeners();
        }

        public PlayerView CreatePlayerView(int goPostfix)
        {
            var instance = GameObject.Instantiate<PlayerView>(_playerViewPrefab
                , _playerViewsContainer.transform);
            instance.gameObject.name = "PlayerView " + goPostfix;

            _playerViews.Add(instance);

            UpdatePlayerViewsPosition();

            return instance;
        }

        public void UpdatePlayerViewsPosition()
        {
            int count = _playerViews.Count;

            float offsetStep = 1.0f / count;

            float offset = offsetStep * .5f;
            for(int i = 0; i < count; i++)
            {
                _playerViews[i].SetViewAnchors(_playerViewsOrientation, offset);
                offset += offsetStep;
            }
        }

        private void AddListeners()
        {
            
        }
        private void RemoveListeners()
        {
            
        }
    }
}
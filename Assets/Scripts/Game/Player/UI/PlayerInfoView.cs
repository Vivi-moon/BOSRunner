using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerInfoView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Text _playerName;

        [SerializeField] private string _playerNameFormat = "Player: {0}";
        

        public void SetName(string name)
        {
            _playerName.text = string.Format(_playerNameFormat, name);
        }
    }
}
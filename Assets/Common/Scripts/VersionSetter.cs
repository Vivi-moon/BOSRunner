using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class VersionSetter : MonoBehaviour
    {
        [SerializeField] private Text _versionLabel;

        private void Start()
        {
            if (_versionLabel == null) { return; }
            _versionLabel.text = $"Version: {Application.version}";
        }
    }
}
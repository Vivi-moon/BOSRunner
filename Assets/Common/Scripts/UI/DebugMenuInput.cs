using UnityEngine;

namespace Common.Game
{
    public class DebugMenuInput : MonoBehaviour
    {
        [SerializeField] private bool _enabledOnStart;
        [Space]
        [SerializeField] private GameObject _target;
        [Space]
        [SerializeField] private KeyCode _key = KeyCode.D;
        [SerializeField] private int _touchesCount = 3;


        private void Start()
        {
#if !DEVELOPMENT
            SetEnabled(false);
#else
            SetEnabled(_enabledOnStart);
#endif
        }

        private void Update()
        {
#if !DEVELOPMENT
            return;
#endif
            if (_target == null) { return; }

            if (Input.GetKeyDown(_key))
            {
                SwitchEnabled();
                return;
            }

            if (Input.touchCount == _touchesCount)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    SwitchEnabled();
                    return;
                }
            }
        }

        private void SwitchEnabled()
        {
            SetEnabled(!_target.activeSelf);
        }

        private void SetEnabled(bool enabled)
        {
            _target.SetActive(enabled);
        }
    }
}
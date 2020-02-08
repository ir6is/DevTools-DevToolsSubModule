using UnityEngine;
using UnityEngine.UI;

namespace UnityDevTools.Console
{
    public class Console : MonoBehaviour
    {
        #region data

        private const int _mouseInputMaxSize = 4;
        private const string _resourcePath = "Console";
        private static Console _instance;

#pragma warning disable CS0649

        [SerializeField]
        private Button _openBtn, _closeLongConsoleBtn, _hideBtn, _closeConsole;

#pragma warning restore CS0649

        private Vector3[] _mouseInputs;
        private int _mouseInputCarret = -1;

        #endregion

        #region interface

        public static Console Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<Console>(_resourcePath));
                    _instance.name = _instance.name.Replace("(Clone)", "");
                }

                return _instance;
            }
        }

        public ShortConsole ShortConsole { get; private set; }
        public LongConsole FullScreenConsole { get; private set; }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }

            ShortConsole = GetComponentInChildren<ShortConsole>(true);
            ShortConsole.Initialize();

            FullScreenConsole = GetComponentInChildren<LongConsole>(true);
            FullScreenConsole.Initialize();
            FullScreenConsole.CommandRaised += HubScene.LoadHubScene;
            FullScreenConsole.CommandRaised += Version.PrintVersion;

            _openBtn.onClick.AddListener(() =>
            {
                FullScreenConsole.gameObject.SetActive(true);
                ShortConsole.gameObject.SetActive(false);
            });

            _closeLongConsoleBtn.onClick.AddListener(() =>
            {
                FullScreenConsole.gameObject.SetActive(false);
                ShortConsole.gameObject.SetActive(true);
            });

            _hideBtn.onClick.AddListener(() =>
            {
                ShortConsole.gameObject.SetActive(false);
            });

            _closeConsole.onClick.AddListener(() =>
            {
                ShortConsole.gameObject.SetActive(false);
            });

            _mouseInputs = new Vector3[_mouseInputMaxSize];

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseInputCarret = ++_mouseInputCarret % _mouseInputMaxSize;
                _mouseInputs[_mouseInputCarret] = Input.mousePosition;

                var firstClick = _mouseInputs[(_mouseInputCarret + 1) % _mouseInputMaxSize];
                var secondClick = _mouseInputs[(_mouseInputCarret + 2) % _mouseInputMaxSize];
                var thirdClick = _mouseInputs[(_mouseInputCarret + 3) % _mouseInputMaxSize];
                var fourth = _mouseInputs[_mouseInputCarret];

                if (firstClick.x <= Screen.width/2 && firstClick.y >= Screen.height/2)
                {
                    if (secondClick.x >= Screen.width/2 && secondClick.y >= Screen.height/2)
                    {
                        if (thirdClick.x <= Screen.width/2 && thirdClick.y <= Screen.height/2)
                        {
                            if (fourth.x >= Screen.width / 2 && fourth.y <= Screen.height / 2)
                            {
                                for (int i = 0; i < _mouseInputs.Length; i++)
                                {
                                    _mouseInputs[i] = Vector3.zero;
                                }

                                if (!FullScreenConsole.isActiveAndEnabled)
                                {
                                    ShortConsole.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}

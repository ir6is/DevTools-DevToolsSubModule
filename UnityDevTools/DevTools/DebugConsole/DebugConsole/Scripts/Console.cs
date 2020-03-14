using UnityEngine;
using UnityEngine.UI;

namespace UnityDevTools.Console
{
    public class Console : MonoBehaviour
    {
        #region data

        private const int _mouseInputMaxSize = 4;

#pragma warning disable CS0649

        [SerializeField]
        private Button _openBtn, _closeLongConsoleBtn, _closeConsole;
        [SerializeField]
        private string[] _deviseIds;
        [SerializeField]
        private bool _allDevisesSupport = true;

#pragma warning restore CS0649

        private const string _resourcePath = "Console";
        private static Console _instance;

        private Vector3[] _mouseInputs;
        private int _mouseInputCarret = -1;
        private bool _idContains;

        #endregion

        #region interface

        public static Console Instance
        {
            get
            {
                if (!_instance)
                {
                     _instance=FindObjectOfType<Console>();

                    if (!_instance)
                    {
                        _instance = Instantiate(Resources.Load<Console>(_resourcePath));
                        _instance.name = _instance.name.Replace("(Clone)", "");
                    }
                }

                return _instance;
            }
        }

        public ShortConsole ShortConsole { get; private set; }
        public LongConsole LongConsole { get; private set; }

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

            foreach (var item in _deviseIds)
            {
                if (SystemInfo.deviceUniqueIdentifier == item)
                {
                    _idContains = true;
                    break;
                }
            }

            ShortConsole = GetComponentInChildren<ShortConsole>(true);
            ShortConsole.Initialize();

            LongConsole = GetComponentInChildren<LongConsole>(true);
            LongConsole.Initialize();

            _openBtn.onClick.AddListener(() =>
            {
                LongConsole.gameObject.SetActive(true);
                ShortConsole.gameObject.SetActive(false);
            });

            _closeLongConsoleBtn.onClick.AddListener(() =>
            {
                LongConsole.gameObject.SetActive(false);
                ShortConsole.gameObject.SetActive(true);
            });

            _closeConsole.onClick.AddListener(() =>
            {
                ShortConsole.gameObject.SetActive(false);
            });

            _mouseInputs = new Vector3[_mouseInputMaxSize];
        }

        private void Update()
        {
            if (_allDevisesSupport || _idContains)
            {
                CheckOpen();
            }
        }

        #endregion

        #region interface

        private void CheckOpen()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseInputCarret = ++_mouseInputCarret % _mouseInputMaxSize;
                _mouseInputs[_mouseInputCarret] = Input.mousePosition;

                var firstClick = _mouseInputs[(_mouseInputCarret + 1) % _mouseInputMaxSize];
                var secondClick = _mouseInputs[(_mouseInputCarret + 2) % _mouseInputMaxSize];
                var thirdClick = _mouseInputs[(_mouseInputCarret + 3) % _mouseInputMaxSize];
                var fourth = _mouseInputs[_mouseInputCarret];

                if (firstClick.x <= Screen.width / 2 && firstClick.y >= Screen.height / 2)
                {
                    if (secondClick.x >= Screen.width / 2 && secondClick.y >= Screen.height / 2)
                    {
                        if (thirdClick.x <= Screen.width / 2 && thirdClick.y <= Screen.height / 2)
                        {
                            if (fourth.x >= Screen.width / 2 && fourth.y <= Screen.height / 2)
                            {
                                for (int i = 0; i < _mouseInputs.Length; i++)
                                {
                                    _mouseInputs[i] = Vector3.zero;
                                }

                                if (!LongConsole.isActiveAndEnabled)
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

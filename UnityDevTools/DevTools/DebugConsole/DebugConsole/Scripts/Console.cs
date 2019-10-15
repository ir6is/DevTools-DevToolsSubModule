using UnityEngine;
using UnityEngine.UI;

namespace UnityDevTools.Console
{
	public class Console : MonoBehaviour
	{

        #region data

        [SerializeField]
        private Button _openBtn, _closeLongConsoleBtn, _hideBtn, _closeConsole, _hiddenConsole;

        private const string _resourcePath = "Console";
		private static Console _instance;

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
				_hiddenConsole.gameObject.SetActive(true);
			});

			_closeConsole.onClick.AddListener(() =>
			{
				ShortConsole.gameObject.SetActive(false);
			});

			_hiddenConsole.onClick.AddListener(() =>
			{
				ShortConsole.gameObject.SetActive(true);
				_hiddenConsole.gameObject.SetActive(false);
			});
		}

		#endregion

		#region interface

		#endregion
	}
}

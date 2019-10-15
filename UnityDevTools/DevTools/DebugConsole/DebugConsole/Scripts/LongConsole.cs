using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace UnityDevTools.Console
{
	public class LongConsole : MonoBehaviour
	{
		#region data

		[Serializable]
		private class LogViewSettings
		{
			public LogType Type = LogType.Log;
			public Color TextColor = Color.white;
			public bool ShowStack = false;
		}

		/// <summary>
		/// Max lenght _contentText.text view
		/// </summary>
		private const int MaxCharAmount = 16000;

		[Tooltip("Logs settings")]
		[SerializeField]
		private LogViewSettings[] _logViewSettings = new LogViewSettings[1];

		[Tooltip("Ui content viewer when console open")]
		[SerializeField]
		private Text _uiContentTextOpen;
		[SerializeField]
		private InputField _executeInputField;
		[SerializeField]
		private Toggle _autoScrollTogle;

		[SerializeField]
		private Button _executeBtn, _clearLog;

		private Queue<string> _messages;
		private StringBuilder _messageBuilder;
		private ScrollRect _scrollRect;

		#endregion

		#region interfaces

		public event EventHandler<string> CommandRaised;

		public void Initialize()
		{
			_messages = new Queue<string>();
			_messageBuilder = new StringBuilder
			{
				Capacity = MaxCharAmount
			};

			Application.logMessageReceived += OnLogReceived;
			_scrollRect = GetComponentInChildren<ScrollRect>();
			_executeBtn.onClick.AddListener(OnExecuteClick);
			_clearLog.onClick.AddListener(OnClearLogClick);
			OnClearLogClick();
		}

		#endregion

		#region implementation

		private void OnClearLogClick()
		{
			_uiContentTextOpen.text = string.Empty;
			_messages.Clear();
		}

		private void OnExecuteClick()
		{
			CommandRaised?.Invoke(this,_executeInputField.text);
		}

		private void OnLogReceived(string condition, string stackTrace, LogType type)
		{
			foreach (LogViewSettings currLogViewSettings in _logViewSettings)
			{
				if (currLogViewSettings.Type == type)
				{
					string newMessage = string.Format("<color=#{0}>{1}</color>\n",
						ColorUtility.ToHtmlStringRGB(currLogViewSettings.TextColor),
                        condition);

					if (currLogViewSettings.ShowStack)
					{
						newMessage += string.Format("<size={0}><color=#{1}>------\n{2}------\n</color></size>",
							_uiContentTextOpen.fontSize * 0.5f,
							ColorUtility.ToHtmlStringRGB(currLogViewSettings.TextColor), stackTrace);
					}

					_messages.Enqueue(newMessage);

					var visibelMessageCount = 0;
					_messageBuilder.Length = 0;

					//Заполнение видымых месседжей
					foreach (var item in _messages)
					{
						if (_messageBuilder.Length + item.Length < MaxCharAmount)
						{
							visibelMessageCount++;
							_messageBuilder.Append(item);
						}
						else
						{
							break;
						}
					}

					for (int i = 0; i < _messages.Count - visibelMessageCount; i++)
					{
						_messages.Dequeue();
					}

					_uiContentTextOpen.text = _messageBuilder.ToString();

					if (_autoScrollTogle.isOn)
					{
						_scrollRect.verticalNormalizedPosition = 0;
					}
				}
			}
		}

		#endregion
	}
}
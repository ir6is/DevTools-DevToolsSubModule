using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace UnityDevTools.Console
{
    public class ShortConsole : MonoBehaviour
    {
        [Tooltip("Ui content viewer when content close")]
        [SerializeField]
        private Text _uiContentTextClose;
        [Tooltip("with how many frames to take the average")]
        [SerializeField]
        private int _maxFpsBuferSize = 20;
        [SerializeField]
        private Button _clearBtn;

        private Queue<int> _FPSBufer;
        private StringBuilder _messageBuilder;

        private Dictionary<string, string> _keyAndValueView = new Dictionary<string, string>();

        private static string[] _stringsFrom00To99 = {
        "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
        "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
        "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
        "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
        "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
        "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
        "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
        "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
    };

        public void Initialize()
        {
            _FPSBufer = new Queue<int>();
            _keyAndValueView = new Dictionary<string, string>();
            _messageBuilder = new StringBuilder();

            _clearBtn.onClick.AddListener(Clear);
        }

        private void Update()
        {
            ShowCalculatedFps();

            foreach (KeyValuePair<string, string> pair in _keyAndValueView)
            {
                _messageBuilder.Append(pair.Key);
                _messageBuilder.Append(" : ");
                _messageBuilder.Append(pair.Value);
                _messageBuilder.Append("\n");
            }

            _uiContentTextClose.text = _messageBuilder.ToString();
        }


        /// <summary>
        /// Add string which been visualising in close console, if key exist, value will update.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddStringView(string key, string value)
        {
            if (_keyAndValueView.ContainsKey(key))
            {
                _keyAndValueView[key] = value;
            }
            else
            {
                _keyAndValueView.Add(key, value);
            }
        }

        /// <summary>
        /// Remove string which not been visualising in close console
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Remove(string key)
        {
            _keyAndValueView.Remove(key);
        }

        /// <summary>
        ///  Remove all strings which been visualising in close console
        /// </summary>
        public void Clear()
        {
            _keyAndValueView.Clear();
            _uiContentTextClose.text = string.Empty;
        }

        private void ShowCalculatedFps()
        {
            _FPSBufer.Enqueue((int)(1 / Time.deltaTime));
            if (_FPSBufer.Count > _maxFpsBuferSize)
            {
                _FPSBufer.Dequeue();
            }
            int sum = 0;
            foreach (var item in _FPSBufer)
            {
                sum += item;
            }
            _messageBuilder.Length = 0;
            _messageBuilder.Append("FPS = ");
            _messageBuilder.Append(_stringsFrom00To99[Mathf.Clamp(sum / _FPSBufer.Count, 0, 99)]);
            _messageBuilder.Append("\n");
        }

    }
}
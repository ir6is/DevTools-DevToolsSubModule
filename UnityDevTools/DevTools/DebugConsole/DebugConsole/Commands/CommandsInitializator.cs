using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDevTools.Console
{
    /// <summary>
    /// CommandsInitializator.
    /// </summary>
    public class CommandsInitializator : MonoBehaviour
    {
#pragma warning disable CS0649

        [SerializeField]
        private Command[] _commands;

#pragma warning restore CS0649

        private void Start()
        {
            foreach (var item in _commands)
            {
                Console.Instance.LongConsole.CommandRaised += (s, a) => item.OnCommandRaised(s, a);
            }

            Console.Instance.LongConsole.CommandRaised += (s, a) =>

            {
                if (a == "Help")
                {
                    Debug.Log("Available Commands");
                    foreach (var item in _commands)
                    {
                        Debug.Log(item.CommandName);
                    }
                }
            };
        }
    }
}
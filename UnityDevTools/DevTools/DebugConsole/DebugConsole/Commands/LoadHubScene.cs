using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityDevTools.Console
{
    /// <summary>
    /// LoadHubScene.
    /// </summary>
     public class LoadHubScene : Command
    {
        public override string CommandName => "LoadHubScene";
        public override bool OnCommandRaised(object sender, string command)
        {
            if (command == CommandName)
            {
                SceneManager.LoadScene(HubScene.HubSceneName);
                return true;
            }

            return base.OnCommandRaised(sender, command);
        }
    }
}
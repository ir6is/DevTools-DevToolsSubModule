using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDevTools.Console
{
    /// <summary>
    /// Command.
    /// </summary>
    public class Command : MonoBehaviour
    {
        public virtual string CommandName { get;}
        public virtual bool OnCommandRaised(object sender, string command)
        {
            return false;
        }
    }
}
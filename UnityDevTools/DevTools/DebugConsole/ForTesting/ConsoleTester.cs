using System.Collections;
using System.Collections.Generic;
using UnityDevTools.Console;
using UnityEngine;

public class ConsoleTester : MonoBehaviour {

    private int _debugCounter = 0;

    string debugString = "";
	void Start () {
        StartCoroutine(PrintDebug());

        Console.Instance.ShortConsole.AddStringView("debugString", debugString);
        Console.Instance.FullScreenConsole.CommandRaised += OnExecuteListener;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator PrintDebug()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _debugCounter++;
            debugString = _debugCounter.ToString();

          print(_debugCounter.ToString());
        }
    }

    private void OnExecuteListener(object d,string s)
    {
        if (s == "s")
        {
            Console.Instance.ShortConsole.AddStringView("DebugExecute", Time.time.ToString()); 
        }
    }

    public void BtnCliCktest()
    {
        print("Click");
    }
}

 
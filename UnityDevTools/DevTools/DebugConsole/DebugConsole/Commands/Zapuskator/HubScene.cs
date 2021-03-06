﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityDevTools.Console
{
    internal class HubScene : MonoBehaviour
    {
        public const string HubSceneName = "HubScene";

#pragma warning disable CS0649

        public Transform content;
        public Button btnPrefab;

#pragma warning restore CS0649

        void Start()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            } 

            foreach (var scene in scenes)
            {
                 var curBtn = Instantiate(btnPrefab, content);
                curBtn.onClick.AddListener(() => SceneManager.LoadScene(scene));
                curBtn.GetComponentInChildren<Text>().text = (scene);
            }
        } 
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;

public class BuildPostProcess : MonoBehaviour
{

	[PostProcessBuild]
	public static void OnBuildComplete(BuildTarget buildTarget, string pathToBuiltProject)
	{
		if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.Android)
		{
			float version;
			if (float.TryParse(PlayerSettings.bundleVersion, out version))
			{
				version++;
				string versionString = version.ToString();
				PlayerSettings.bundleVersion = versionString;

				string additionalString = "Version_" + versionString;


				if (buildTarget == BuildTarget.iOS)
				{
					System.Threading.Thread.Sleep(2000);//Unity use directory some time
					Directory.Move(pathToBuiltProject, pathToBuiltProject + additionalString);
				}
				if (buildTarget == BuildTarget.Android)
				{
					FileInfo fileInfo = new FileInfo(pathToBuiltProject);
					string name = fileInfo.Name;
					string existension = fileInfo.Extension;
					string path = fileInfo.Directory.FullName;

					string newPath = Path.Combine(path, name) + additionalString + existension;
					// File.Move(pathToBuiltProject, newPath); 
				}
			}
		}
	}
}

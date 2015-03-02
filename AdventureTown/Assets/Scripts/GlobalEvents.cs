using UnityEngine;
using System;
using System.Collections;

public class GlobalEvents : MonoBehaviour 
{
	public static event Action OnBootUp;
	public static event Action OnShutdown;
	public static event Action<string> OnLevelLoaded;

	void Start () 
	{
		DontDestroyOnLoad(this);
		if (OnBootUp != null)
			OnBootUp();
	}

	void OnLevelWasLoaded(int level)
	{
		if (OnLevelLoaded != null)
			OnLevelLoaded(Application.loadedLevelName);
	}

	void OnApplicationQuit()
	{
		if (OnShutdown != null)
			OnShutdown();
	}
}

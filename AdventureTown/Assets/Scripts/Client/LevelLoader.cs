using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour 
{
    private static LevelLoader _instance;
    public static LevelLoader Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
        _instance = this;
    }

    public IEnumerator LoadLevel (string level)
    {        
        // We need to stop receiving because first the level must be loaded first.
        // Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
        //uLink.Network.isMessageQueueRunning = false;
        
        //Load the loading scene here
        //
        //

        // All network views loaded from a level will get a prefix into their NetworkViewID.
        // This will prevent old updates from clients leaking into a newly created scene.
        yield return Application.LoadLevelAsync(level);

        // Allow receiving data again
        //uLink.Network.isMessageQueueRunning = true;
    }
}

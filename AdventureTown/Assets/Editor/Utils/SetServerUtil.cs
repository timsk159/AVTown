using UnityEngine;
using UnityEditor;
using System.Collections;

public class SetServerUtil : MonoBehaviour 
{
    [MenuItem("Utils/SetServer")]
    static void SetServer()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "IsServer");
    }
}

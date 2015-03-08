using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public Text coinsText;
    public Text woodText;

    void Update()
    {
        coinsText.text = CharacterData.Instance.coins.ToString();
        woodText.text = CharacterData.Instance.wood.ToString();
    }

    void OnGUI()
    {
        if(GUILayout.Button("DebugEarn"))
        {
            ClientToServerConnector.Instance.Debug_Earn();
        }
        if (GUILayout.Button("DebugSpend"))
        {
            ClientToServerConnector.Instance.Debug_Spend();
        }
        if(GUILayout.Button("HACKY EARN!"))
        {
            CharacterData.Instance.coins += 1000;
        }
    }
}

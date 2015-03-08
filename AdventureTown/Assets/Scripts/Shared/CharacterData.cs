using UnityEngine;
using System.Collections;

public class CharacterData : uLink.MonoBehaviour
{
    public int coins;
    public int wood;

    private static CharacterData _instance;
    public static CharacterData Instance
    {
        get { return _instance; }
    }

    void uLink_OnSerializeNetworkViewOwner(uLink.BitStream stream, uLink.NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Write<int>(coins);
            stream.Write<int>(wood);
        }
        else
        {
            coins = stream.Read<int>();
            wood = stream.Read<int>();
        }
    }

    void uLink_OnNetworkInstantiate(uLink.NetworkMessageInfo info)
    {
        //Initial data available via: info.networkView.initialData

        coins = info.networkView.initialData.Read<int>();
        wood = info.networkView.initialData.Read<int>();
        DontDestroyOnLoad(this);
        _instance = this;
    }

#if IsServer

    [RPC]
    void DebugEarnRequest()
    {
        coins += 25;
        wood += 25;
    }

    [RPC]
    void DebugSpendRequest()
    {
        if(coins >= 25)
        {
            coins -= 25;
        }
        if(wood >= 25)
        {
            wood -= 25;
        }
    }

    [RPC]
    void PlaceBuildingRequest(int buildingID)
    {

    }

    [RPC]
    void SellBuildingRequest(int buildingInstanceID)
    {

    }

#endif
}
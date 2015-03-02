using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraSettings : ISaveable
{
	public float scrollSpeed;
	public float zoomSpeed;

	public byte[] Serialize()
	{
		throw new System.NotImplementedException();
	}

	public void Deserialize(byte[] data)
	{
		throw new System.NotImplementedException();
	}
}

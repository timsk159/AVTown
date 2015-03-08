using UnityEngine;
using System.Collections;

public interface ISaveable 
{
	byte[] Serialize();
	void Deserialize(byte[] data);
}

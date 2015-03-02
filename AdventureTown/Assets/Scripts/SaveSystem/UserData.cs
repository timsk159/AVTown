using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UserData : ISaveable
{
	string userName;
	List<Advenuterer> adventurers;
	List<Hero> heroes;




	public byte[] Serialize()
	{
		throw new System.NotImplementedException();
	}

	public void Deserialize(byte[] data)
	{
		throw new System.NotImplementedException();
	}
}

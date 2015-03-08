using UnityEngine;
using System.Collections;

public class Save
{
	UserData _userData;
	MapData _mapData;
	CameraSettings _camSettings;

	public UserData userData
	{
		get
		{
			return _userData;
		}
	}
	public MapData mapData
	{
		get
		{
			return _mapData;
		}
	}
	public CameraSettings camSettings
	{
		get
		{
			return _camSettings;
		}
	}

	public Save(byte[] userDataBytes, byte[] mapDataBytes, byte[] camSettingsBytes)
	{
		_userData = new UserData();
		_userData.Deserialize(userDataBytes);

		_mapData = new MapData();
		_mapData.Deserialize(mapDataBytes);

		_camSettings = new CameraSettings();
		_camSettings.Deserialize(camSettingsBytes);
	}
}

public class SaveSystem 
{
	private static SaveSystem _instance;

	public static SaveSystem instance
	{
		get
		{
			if (_instance == null)
				_instance = new SaveSystem();
			return _instance;
		}
	}

	public void Save()
	{

	}

	public void Load()
	{

	}

}

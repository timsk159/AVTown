using UnityEngine;
using System.Collections;
using System.Linq;

public static class GameObjectExt 
{
	public static bool HasInterface<T>(this GameObject g) where T : class
	{
		MonoBehaviour[] list = g.GetComponents<MonoBehaviour>();
		if (list == null || list.Length == 0)
			return false;
		if (list.Any(e => e is T))
			return true;
		else
			return false;
	}

	public static bool TryGetInterface<T>(this GameObject g, out T inter) where T : class
	{
		MonoBehaviour[] list = g.GetComponents<MonoBehaviour>();
		if (list == null || list.Length == 0)
		{
			inter = null;
			return false;
		}
		inter = list.First(e => e is T) as T;
		if (inter == null)
			return false;
		else
			return true;
	}
}

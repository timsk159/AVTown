using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TapRecognizer))]
public class CameraSelection : MonoBehaviour 
{
	ISelectable selectedObj;
	void OnTap(TapGesture gesture)
	{
		Ray ray = Camera.main.ScreenPointToRay(gesture.Position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (selectedObj != null)
			{
				selectedObj.DeSelected();
				selectedObj = null;
			}
			ISelectable selectable;
			if (hit.transform.gameObject.TryGetInterface<ISelectable>(out selectable))
			{
				selectable.Selected();
				selectedObj = selectable;
			}
		}
	}

}

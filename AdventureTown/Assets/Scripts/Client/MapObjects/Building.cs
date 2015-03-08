using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour, ISelectable
{
	[SerializeField]
	protected GameObject popupPrefab;

	Popup currentPopup;

	public void Selected()
	{
		ShowUIPopUp();
	}

	public void DeSelected()
	{
		CloseUIPopUp();
	}

	public void ShowUIPopUp()
	{
		if (popupPrefab != null)
		{
			if (currentPopup == null)
			{
				var go = (GameObject)GameObject.Instantiate(popupPrefab);
				currentPopup = go.GetComponent<Popup>();
				var canvas = GameObject.FindWithTag("Canvas");
				go.GetComponent<RectTransform>().SetParent(canvas.transform);
			}
			else
			{
				currentPopup.Open();
			}
		}
	}

	public void CloseUIPopUp()
	{
		if (currentPopup != null)
		{
			currentPopup.Close();
		}
	}

	protected virtual void OnSelected() { }

	protected virtual void OnDeSelected() { }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicHousePopup : Popup 
{
	Button closeButton;

	protected override void InternalStart()
	{
		closeButton = transform.Find("CloseButton").GetComponent<Button>();
		closeButton.onClick.AddListener(CloseButtonClicked);
	}

	protected override void InternalOnDestroy()
	{
		closeButton.onClick.RemoveAllListeners();
	}

	public override void Open()
	{
		animator.SetTrigger("Show");
	}

	public override void Close()
	{
		animator.SetTrigger("Hide");		
	}

	void CloseButtonClicked()
	{
		Close();
	}
}

using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
	[SerializeField]
	CameraSettings cameraSettings;

	float desiredZoom;
	bool zoomingIn;
	bool zoomingOut;

	void LateUpdate()
	{
		DoScrolling();
		DoMouseScrolling();
		DoZooming();
	}

	void DoScrolling()
	{
		var horiInput = Input.GetAxis("Horizontal");
		var vertInput = Input.GetAxis("Vertical");

		if (horiInput != 0 || vertInput != 0)
		{
			if (Input.GetAxis("Shift") != 0)
			{
				transform.Translate(new Vector3(horiInput, 0, vertInput) * Time.deltaTime * cameraSettings.scrollSpeed * 2.0f, Space.World);

			}
			else
			{
				transform.Translate(new Vector3(horiInput, 0, vertInput) * Time.deltaTime * cameraSettings.scrollSpeed, Space.World);
			}
		}
	}

	void DoMouseScrolling()
	{

	}

	void DoZooming()
	{
		var scrollInput = Input.GetAxis("Mouse ScrollWheel");
		if (zoomingIn && Camera.main.orthographicSize != desiredZoom)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize - 1, cameraSettings.zoomSpeed * Time.deltaTime);
			if (Camera.main.orthographicSize <= desiredZoom)
			{
				zoomingIn = false;
			}
		}
		else if (zoomingOut && Camera.main.orthographicSize != desiredZoom)
		{
			Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize + 1, cameraSettings.zoomSpeed * Time.deltaTime);
			if (Camera.main.orthographicSize >= desiredZoom)
			{
				zoomingOut = false;
			}
		}
		if(scrollInput != 0)
		{
			var sign = Mathf.Sign(scrollInput);
			if (sign < 0)
				zoomingIn = true;
			else if (sign > 0)
				zoomingOut = true;
			desiredZoom = Camera.main.orthographicSize + (2 * sign);
		}
	}
}

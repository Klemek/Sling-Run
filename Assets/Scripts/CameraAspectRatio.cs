﻿using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
	public Vector2 ratio;
	
	// Use this for initialization
	private void Start ()
	{
		var expectedRatio = ratio.x / ratio.y;
		var mainCamera = GetComponent<Camera>();
		var currentRatio = mainCamera.scaledPixelWidth / (float)mainCamera.scaledPixelHeight;
		var rect = mainCamera.rect;
		if (currentRatio - expectedRatio < - float.Epsilon) // <
		{
			rect.height = currentRatio / expectedRatio;
			rect.position = new Vector2(0,(1f-rect.height)/2f);
		}else if (currentRatio - expectedRatio > float.Epsilon) // >
		{
			rect.width = expectedRatio / currentRatio;
			rect.position = new Vector2((1f-rect.width)/2f,0);
		}
		mainCamera.rect = rect;
	}
}
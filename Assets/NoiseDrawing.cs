using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDrawing : MonoBehaviour
{
	public Renderer textureRenderer;

	public void DrawNoise(Vector2 regionSize, Vector3[] vertices)
	{
		int width = (int)regionSize.x + 1;
		int length = (int)regionSize.y + 1;

		Texture2D texture = new Texture2D(width, length);

		Color[] colorMap = new Color[width * length];
		for (int index = 0, z = 0; z < length; z++)
		{
			for (int x = 0; x < width; x++, index++)
			{
				colorMap[index] = Color.Lerp(Color.black, Color.white, vertices[index].y);
			}
		}

		texture.SetPixels(colorMap);
		texture.Apply();

		textureRenderer.sharedMaterial.mainTexture = texture;
		textureRenderer.enabled = true;
	}
}
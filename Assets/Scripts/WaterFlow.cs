using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
	public float scrollSpeedX = 0.1f; // X�� �̵� �ӵ�
	public float scrollSpeedY = 0.1f; // Y�� �̵� �ӵ�
	public Renderer rend;

	void Start()
	{
		rend = GetComponent<Renderer>();
	}

	void Update()
	{
		float offsetX = Time.time * scrollSpeedX;
		float offsetY = Time.time * scrollSpeedY;
		rend.material.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));
	}
}

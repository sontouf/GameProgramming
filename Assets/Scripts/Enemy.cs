using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		EnemyManager.instance.enemies.Add(this);
	}

	private void OnDestroy()
	{
		EnemyManager.instance.enemies.Remove(this);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Base"))
			other.GetComponent<Life>().amount = 0;
	}
}

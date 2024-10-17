using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
	public float damage;

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Trgger Damager");
		Destroy(gameObject);

		Life life = other.GetComponent<Life>();

		if (life != null)
		{
			life.amount -= damage;
		}
	}
}

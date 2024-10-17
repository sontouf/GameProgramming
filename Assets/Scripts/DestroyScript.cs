using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
	public float delay;
	// Start is called before the first frame update
	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
		Destroy(other.gameObject);
	}
}

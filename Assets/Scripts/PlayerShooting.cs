using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	// Start is called before the first frame update

	public GameObject[] prefab;
	public GameObject shootPoint;
	int count = 0;
	// Update is called once per frame

	public void OnFire()
	{
		GameObject clone;
		if (count >= 5)
		{
			count = 0;
			clone = Instantiate(prefab[1]);
			clone.transform.position = shootPoint.transform.position;
			clone.transform.rotation = shootPoint.transform.rotation;
			//.GetComponent<ForwardMovement>().speed = 3;
		}
		else
		{
			count++;
			clone = Instantiate(prefab[0]);
			clone.transform.position = shootPoint.transform.position;
			clone.transform.rotation = shootPoint.transform.rotation;
		}
	}
}

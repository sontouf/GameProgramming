using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	// Start is called before the first frame update

	public GameObject[] prefab;
	public GameObject shootPoint;
	public GameObject powerUpParticlesPrefab;
	private PlayerMovement playerMovement;

	int count;
	// Update is cald once per frame

	private void Awake()
	{
		count = 0;
		playerMovement = GetComponent<PlayerMovement>();
	}

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

	public void OnEnemyKilled()
	{
		Debug.Log("PowerUp()");
		StartCoroutine(PowerUp());
	}
	private IEnumerator PowerUp()
	{
		powerUpParticlesPrefab.GetComponent<ParticleSystem>().Play();

		// 플레이어 속도와 공격력 증가
		playerMovement.speed *= 1.5f;
		playerMovement.rotationSpeed *= 1.5f;

		// 일정 시간 동안 효과 유지
		yield return new WaitForSeconds(5f);

		playerMovement.speed /= 1.5f;
		playerMovement.rotationSpeed /= 1.5f;
	}
}

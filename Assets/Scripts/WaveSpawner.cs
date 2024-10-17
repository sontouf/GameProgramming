using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	public GameObject prefab;
	public Transform[] spawnPoints;

	public float startTime;
	public float endTime;
	public float spawnRate;

	private void Awake()
	{
		spawnPoints = GetComponentsInChildren<Transform>();
	}
	// Start is called before the first frame update
	void Start()
	{
		startTime = 1f;
		endTime = 5f;
		spawnRate = 0.5f;

		if (WavesManager.instance == null || WavesManager.instance.waves == null)
		{
			Debug.LogError("WavesManager 또는 waves 리스트가 null입니다.");
			return;
		}

		Debug.Log("InvokeRepeating 호출");
		WavesManager.instance.AddWave(this);
		InvokeRepeating(nameof(Spawn), startTime, spawnRate);
		Debug.Log("EndSpawner가 호출될 시간: " + endTime);
		Invoke(nameof(EndSpawner), endTime);
	}

	void Spawn()
	{
		GameObject enemy = Instantiate(prefab, transform.position, transform.rotation);
		//GameObject enemy = Instantiate(prefab);
		enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;

	}

	void EndSpawner()
	{
		WavesManager.instance.waves.Remove(this);
		CancelInvoke();
	}
}

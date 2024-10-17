using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WavesGameMode : MonoBehaviour
{
	public Life playerLife;
	public Life playerBaseLife;

	private void Start()
	{
		playerLife.onDeath.AddListener(OnPlayerOrBaseDied);
		playerBaseLife.onDeath.AddListener(OnPlayerOrBaseDied);
		EnemyManager.instance.onChanged.AddListener(CheckWinCondition);
		WavesManager.instance.onChanged.AddListener(CheckWinCondition);
	}

	private void Update()
	{
		//Debug.Log("enemies.Count : " + EnemyManager.instance.enemies.Count + "WavesManager.instance.waves.Count : " + WavesManager.instance.waves.Count);
		if (EnemyManager.instance.enemies.Count <= 0 && WavesManager.instance.waves.Count <= 0)
		{
			SceneManager.LoadScene("WinScreen");
		}

		if (playerLife.amount <= 0 || playerBaseLife.amount <= 0)
		{
			SceneManager.LoadScene("LoseScreen");
		}
	}

	void OnPlayerOrBaseDied()
	{
		SceneManager.LoadScene("LoseScreen");
	}

	void CheckWinCondition()
	{
		if (EnemyManager.instance.enemies.Count <= 0 && WavesManager.instance.waves.Count <= 0)
		{
			SceneManager.LoadScene("WinScreen");
		}
	}
}

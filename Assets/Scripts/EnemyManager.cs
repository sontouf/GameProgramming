using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
	public List<Enemy> enemies;
	public UnityEvent onChanged;
	public static EnemyManager instance;

	public void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);
		onChanged.Invoke();
	}

	public void RemoveEnemy(Enemy enemy)
	{
		enemies.Remove(enemy);
		onChanged.Invoke();
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			print("Duplicated EnemyManager)");
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
	public enum EnemyState { GoToBase, AttackBase, ChasePlayer, AttackPlayer }
	public EnemyState currentState;

	private NavMeshAgent agent;

	private void Awake()
	{
		baseTransform = GameObject.Find("Base").transform;
		agent = GetComponentInParent<NavMeshAgent>();
	}

	private void Update()
	{
		if (currentState == EnemyState.GoToBase) { GoToBase(); }
		else if (currentState == EnemyState.AttackBase) { AttackBase(); }
		else if (currentState == EnemyState.ChasePlayer) { ChasePlayer(); }
		else { AttackPlayer(); }
	}

	public Sight sightSensor;
	public Transform baseTransform;
	public float baseAttackDistance;

	void GoToBase()
	{
		agent.isStopped = false;
		bool check = agent.SetDestination(baseTransform.position);
		//Debug.Log("check : " + check);

		NavMeshHit hit;
		//bool isOnNavMesh = NavMesh.SamplePosition(baseTransform.position, out hit, 1.0f, NavMesh.AllAreas);
		//Debug.Log("Base is on NavMesh: " + isOnNavMesh);

		if (NavMesh.SamplePosition(baseTransform.position, out hit, 5.0f, NavMesh.AllAreas))
		{
			baseTransform.position = hit.position;  // Base를 가장 가까운 NavMesh 위치로 이동
			//Debug.Log("Base repositioned to NavMesh.");
		}
		if (sightSensor.detectedObject != null)
		{
			currentState = EnemyState.ChasePlayer;
		}
	}
	void AttackBase()
	{
		agent.isStopped = true;
		LookTo(baseTransform.position);
		Shoot();
	}

	public float playerAttackDistance;
	void ChasePlayer()
	{
		agent.isStopped = false;
		if (sightSensor.detectedObject == null)
		{
			currentState = EnemyState.GoToBase;
			return;
		}

		agent.SetDestination(sightSensor.detectedObject.transform.position);
		float distanceToBase = Vector3.Distance(
			transform.position, sightSensor.detectedObject.transform.position);
		if (distanceToBase < playerAttackDistance)
		{
			currentState = EnemyState.AttackPlayer;
		}
	}
	void AttackPlayer()
	{
		agent.isStopped = true;
		if (sightSensor.detectedObject == null)
		{
			currentState = EnemyState.GoToBase;
			return;
		}

		LookTo(sightSensor.detectedObject.transform.position);
		Shoot();

		float distanceToPlayer = Vector3.Distance(transform.position, sightSensor.detectedObject.transform.position);
		if (distanceToPlayer > playerAttackDistance * 1.1f)
		{
			currentState = EnemyState.ChasePlayer;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, playerAttackDistance);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
	}

	public float lastShootTime;
	public GameObject bulletPrefab;
	public float fireRate;

	void Shoot()
	{
		var timeSinceLastShoot = Time.time - lastShootTime;
		if (timeSinceLastShoot > fireRate)
		{
			lastShootTime = Time.time;
			Instantiate(bulletPrefab, transform.position, transform.rotation);
		}
	}
	void LookTo(Vector3 targetPosition)
	{
		Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
		directionToPosition.y = 0;
		transform.parent.forward = directionToPosition;
	}

}

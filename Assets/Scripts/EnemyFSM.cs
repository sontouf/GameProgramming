using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyFSM : MonoBehaviour
{
	public enum EnemyState { GoToBase, AttackBase, ChasePlayer, AttackPlayer, Retreat }
	public EnemyState currentState;

	private NavMeshAgent agent;

	public Transform retreatPoint;
	public Transform playerTransform;

	private Life lifeComponent; // Life ������Ʈ�� �����Ͽ� ü�� Ȯ��
	public float retreatHealthThreshold = 20f; // �������� �����ϴ� ü�� �Ӱ谪

	public GameObject deathParticlesPrefab;
	public GameObject retreatParticlesPrefab;

	private PlayerShooting playerShooting;

	private void Awake()
	{
		baseTransform = GameObject.Find("Base").transform;
		retreatPoint = GameObject.Find("EnemyBase").transform;
		playerTransform = GameObject.FindWithTag("Player").transform; // �÷��̾� ã��
		playerShooting = playerTransform.GetComponent<PlayerShooting>();
		agent = GetComponentInParent<NavMeshAgent>();
		lifeComponent = GetComponentInParent<Life>(); // Life ������Ʈ ��������
		timeSinceLastShoot = 0f;

		if (lifeComponent != null && deathParticlesPrefab != null)
		{
			lifeComponent.onDeath.AddListener(PlayDeathParticles);
		}
	}

	private void Update()
	{
		if (lifeComponent.amount <= retreatHealthThreshold && currentState != EnemyState.Retreat)
		{
			currentState = EnemyState.Retreat;
			Retreat();
			retreatParticlesPrefab.GetComponent<ParticleSystem>().Play();
		}
		else if (currentState == EnemyState.Retreat)
		{
			float distanceToBase = Vector3.Distance(retreatPoint.position, transform.parent.position);
			// enemy base�� ������� �����ϸ�
			if (distanceToBase < 5f)
			{
				lifeComponent.amount = 100f; // ü�� ȸ��
				currentState = EnemyState.GoToBase; // �ٽ� base�� ���ư���
			}
		}

		switch (currentState)
		{
			case EnemyState.GoToBase:
				GoToBase();
				break;
			case EnemyState.AttackBase:
				AttackBase();
				break;
			case EnemyState.ChasePlayer:
				ChasePlayer();
				break;
			case EnemyState.AttackPlayer:
				AttackPlayer();
				break;
			default:
				break;
		}
		timeSinceLastShoot += Time.deltaTime;


	}

	public Sight sightSensor;
	public Transform baseTransform;
	public float baseAttackDistance;

	void GoToBase()
	{
		agent.isStopped = false;
		agent.SetDestination(baseTransform.position);
		//Debug.Log("check : " + check);

		//NavMeshHit hit;
		//bool isOnNavMesh = NavMesh.SamplePosition(baseTransform.position, out hit, 1.0f, NavMesh.AllAreas);
		//Debug.Log("Base is on NavMesh: " + isOnNavMesh);

		/*		if (NavMesh.SamplePosition(baseTransform.position, out hit, 5.0f, NavMesh.AllAreas))
				{
					baseTransform.position = hit.position;  // Base�� ���� ����� NavMesh ��ġ�� �̵�
					//Debug.Log("Base repositioned to NavMesh.");
				}*/
		//Debug.Log("baseAttackDistance : " + baseAttackDistance + "Distance : " + Vector3.Distance(transform.parent.position, baseTransform.position));
		if (baseAttackDistance >= Vector3.Distance(transform.parent.position, baseTransform.position))
		{
			//Debug.Log("before AttackBase");
			currentState = EnemyState.AttackBase;
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
		Debug.Log("before Shoot");
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

	void Retreat()
	{
		// �÷��̾�� �־����� ���� ���� ����
		agent.isStopped = false;
		agent.SetDestination(retreatPoint.position); // �������� �������� �̵�
		//Debug.Log("Health : " + lifeComponent.amount + ", retreat? : " + check);
	}

	void PlayDeathParticles()
	{
		if (deathParticlesPrefab != null)
		{
			GameObject deathParticlesObject = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);

			// ��ƼŬ �ý����� ������ Play ȣ��
			ParticleSystem deathParticlesSystem = deathParticlesObject.GetComponent<ParticleSystem>();
			if (deathParticlesSystem != null)
			{
				deathParticlesSystem.Play();  // ��ƼŬ ���
			}
		}
		if (playerShooting != null)
		{
			playerShooting.OnEnemyKilled();
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, playerAttackDistance);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, baseAttackDistance);
	}

	public float timeSinceLastShoot;
	public GameObject bulletPrefab;
	public float fireRate;

	void Shoot()
	{
		if (timeSinceLastShoot > fireRate)
		{
			Instantiate(bulletPrefab, transform.position, transform.rotation);
			timeSinceLastShoot = 0;
			currentState = EnemyState.GoToBase;
		}
	}
	void LookTo(Vector3 targetPosition)
	{
		Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
		directionToPosition.y = 0;
		transform.parent.forward = directionToPosition;
	}

}

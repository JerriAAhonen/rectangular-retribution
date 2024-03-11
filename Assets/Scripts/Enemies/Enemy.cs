using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[SerializeField] private float armor;
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Loot lootExperiencePrefab;
	[SerializeField] private float experienceDropForce;

	private float health;
	private Transform target;
	private Rigidbody rb;
	private NavMeshAgent navMeshAgent;
	private int randFrameToUpdateDestinationOn;

	private Action<Enemy> onDeath;

	public void Init(Transform target, Action<Enemy> onDeath)
	{
		this.target = target;
		this.onDeath = onDeath;

		health = maxHealth;
		rb = GetComponent<Rigidbody>();
		navMeshAgent = GetComponent<NavMeshAgent>();

		randFrameToUpdateDestinationOn = UnityEngine.Random.Range(1, 31);

    }

	private void Update()
	{
		if (Time.frameCount % randFrameToUpdateDestinationOn == 0)
		{
			navMeshAgent.SetDestination(target.position);
		}
	}

	/*private void FixedUpdate()
	{
		if (target)
		{
			var dir = (target.position - transform.position).normalized;
			rb.velocity = movementSpeed * Time.fixedDeltaTime * dir;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
		}
	}*/

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<PlayerController>(out var pc))
		{
			// Hurt player
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage - armor;

		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		var lootXp = Instantiate(lootExperiencePrefab, transform.position + Vector3.up * 1f, UnityEngine.Random.rotation);
		lootXp.Init(UnityEngine.Random.insideUnitSphere * experienceDropForce);

		onDeath?.Invoke(this);
		Destroy(gameObject);
	}
}

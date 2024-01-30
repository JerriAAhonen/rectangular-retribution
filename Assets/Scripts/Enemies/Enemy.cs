using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[SerializeField] private float armor;
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Rigidbody lootExperiencePrefab;
	[SerializeField] private float xpDropForce;

	private float health;
	private Transform target;
	private Rigidbody rb;

	private Action<Enemy> onDeath;

	public void Init(Action<Enemy> onDeath)
	{
		this.onDeath = onDeath;
	}

	private void Awake()
	{
		health = maxHealth;
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		target = LevelController.Instance.PlayerController.transform;
	}

	private void FixedUpdate()
	{
		if (target)
		{
			var dir = (target.position - transform.position).normalized;
			rb.velocity = movementSpeed * Time.fixedDeltaTime * dir;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);
		}
	}

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
		var lootXp = Instantiate(lootExperiencePrefab);
		lootXp.MovePosition(transform.position + Vector3.up * 1f);
		lootXp.AddForce(UnityEngine.Random.insideUnitSphere * xpDropForce);

		onDeath?.Invoke(this);
		Destroy(gameObject);
	}
}

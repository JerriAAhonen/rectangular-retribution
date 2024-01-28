using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[SerializeField] private float armor;
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;

	private float health;
	private Transform target;
	private Rigidbody rb;

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
		health -= damage * armor;

		if (health <= 0)
			Destroy(gameObject);
	}
}

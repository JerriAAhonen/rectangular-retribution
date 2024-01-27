using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float maxHealth;
	[SerializeField] private float armor;

	private float health;

	private void Awake()
	{
		health = maxHealth;
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

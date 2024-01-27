using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Rigidbody rb;
	private Vector3 dir;
	private float speed;

	private float destroyAfter = 5f;
	private float aliveFor;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Shoot(Vector3 dir, float speed)
	{
		this.dir = dir;
		this.speed = speed;
		aliveFor = 0f;
	}

	private void Update()
	{
		rb.velocity = dir * speed;

		aliveFor += Time.deltaTime;
		if (aliveFor > destroyAfter)
			ReturnToPool();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
		{
			enemy.TakeDamage(1);
		}

		ReturnToPool();
	}

	private void ReturnToPool()
	{
		ProjectilePool.Instance.Pool.Release(this);
	}
}

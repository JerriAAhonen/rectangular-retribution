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
	private bool hasCollided;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Reset()
	{
		aliveFor = 0f;
		hasCollided = false;
	}

	public void Shoot(Vector3 dir, float speed)
	{
		this.dir = dir;
		this.speed = speed;
		Reset();
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
		if (hasCollided) return;
		hasCollided = true;

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

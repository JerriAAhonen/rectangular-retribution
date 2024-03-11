using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float shootInterval;
	[Header("References")]
	[SerializeField] private Transform ref_shootPoint;
	[SerializeField] private ParticleSystem ps_muzzleFlash;
	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private RotateTowards playerRotator;

	private PlayerController pc;

	private float lastShootTime;

	public void Init(PlayerController pc)
	{
		this.pc = pc;
	}

	private void Update()
	{
		// Shooting completely disabled
		if (!pc.ShootingEnabled) return;

		// Not enough time passed between shots
		if (Time.time - lastShootTime < shootInterval) return;

		var target = FindClosest();

		// No targets on the map
		if (target == null) return;

		// Turn to face the closest enemy
		playerRotator.SetTarget(target);

		// Record time of shot
		lastShootTime = Time.time;
		
		var projectile = ProjectilePool.Instance.Pool.Get();
		projectile.transform.position = ref_shootPoint.position;
		projectile.transform.rotation = ref_shootPoint.rotation;
		projectile.Shoot(ref_shootPoint.forward, 10f);

		ps_muzzleFlash.Play();
	}

	private Transform FindClosest()
	{
		var minDist = 1f;
		var closestDist = float.MaxValue;
		Enemy closestEnemy = null;
		foreach (Enemy enemy in enemySpawner.SpawnedEnemies)
		{
			var dist = (enemy.transform.position - transform.position).sqrMagnitude;
			if (dist < closestDist)
			{
				closestDist = dist;
				closestEnemy = enemy;
			}

			if (dist <= minDist)
				return enemy.transform;
		}

		return closestEnemy.transform;
	}
}

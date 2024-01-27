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

	private PlayerController pc;

	private float lastShootTime;

	public void Init(PlayerController pc)
	{
		this.pc = pc;
	}

	private void Update()
	{
		if (!pc.ShootingEnabled) return;
        
        if (Input.GetMouseButton(0))
		{
			if (Time.time - lastShootTime < shootInterval)
				return;

			lastShootTime = Time.time;

			var projectile = ProjectilePool.Instance.Pool.Get();
			projectile.transform.position = ref_shootPoint.position;
			projectile.transform.rotation = ref_shootPoint.rotation;
			projectile.Shoot(ref_shootPoint.forward, 10f);

			ps_muzzleFlash.Play();
		}
	}
}

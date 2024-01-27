using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : Singleton<ProjectilePool>
{
	[SerializeField] private Projectile projectilePrefab;

	private bool collectionChecks = true;
	private int maxPoolSize = 100;
	private IObjectPool<Projectile> pool;
	public IObjectPool<Projectile> Pool => pool ??= new ObjectPool<Projectile>(
															CreatePooledItem, 
															OnTakeFromPool, 
															OnReturnedToPool, 
															OnDestroyPoolObject, 
															collectionChecks, 
															10, 
															maxPoolSize);

	private Projectile CreatePooledItem()
	{
		var go = Instantiate(projectilePrefab, transform);
		return go;
	}

	// Called when an item is returned to the pool using Release
	private void OnReturnedToPool(Projectile projectile)
	{
		projectile.gameObject.SetActive(false);
	}

	// Called when an item is taken from the pool using Get
	private void OnTakeFromPool(Projectile projectile)
	{
		projectile.gameObject.SetActive(true);
	}

	// If the pool capacity is reached then any items returned will be destroyed.
	// We can control what the destroy behavior does, here we destroy the GameObject.
	void OnDestroyPoolObject(Projectile projectile)
	{
		Destroy(projectile.gameObject);
	}
}

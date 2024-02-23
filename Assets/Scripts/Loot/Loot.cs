using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Loot : MonoBehaviour
{
	private Rigidbody rb;

	public void Init(Vector3 spawnForceAndDir)
	{
		if (!rb)
			rb = GetComponent<Rigidbody>();
		rb.AddForce(spawnForceAndDir, ForceMode.Impulse);
	}

	public virtual void OnPickup(Transform player)
	{
		StartCoroutine(PickupRoutine(player));
	}

	private IEnumerator PickupRoutine(Transform target)
	{
		while (!transform.position.InRangeOf(target.position, 0.2f))
		{
			rb.velocity = (target.position - transform.position).normalized * 4f;
			yield return null;
		}

		OnDestroyLoot();
		Destroy(gameObject);
	}

	protected abstract void OnDestroyLoot();
}

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

	public abstract void OnPickup();
}

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum RotateTowardsMode { PredefinedTarget, Dynamic }

public class RotateTowards : MonoBehaviour
{
	[SerializeField] private RotateTowardsMode mode;
	[HideIf("mode", RotateTowardsMode.Dynamic)]
	[SerializeField] private Transform target;
	[SerializeField] private float speed = 5f;

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	private void Update()
	{
		if (target == null)
			return;

		// Calculate the direction vector
		Vector3 direction = target.position - transform.position;

		// Normalize the direction vector
		direction.Normalize();

		// Calculate the target rotation
		Quaternion targetRotation = Quaternion.LookRotation(direction);

		// Smoothly rotate towards the target rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
	}
}

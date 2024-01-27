using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
	[SerializeField] private float speed = 5f;

	private void Update()
	{
		// Get the mouse position in world space
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(Vector3.up, Vector3.zero);
		if (plane.Raycast(ray, out var distance))
		{
			Vector3 mousePosition = ray.GetPoint(distance);

			// Calculate the direction vector
			Vector3 direction = mousePosition - transform.position;

			// Normalize the direction vector
			direction.Normalize();

			// Calculate the target rotation
			Quaternion targetRotation = Quaternion.LookRotation(direction);

			// Smoothly rotate towards the target rotation
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;

	private PlayerController pc;
	private Rigidbody rb;

	public void Init(PlayerController pc)
	{
		this.pc = pc;
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (!pc.MovementEnabled) return;

		var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		rb.velocity = movementSpeed * Time.deltaTime * input.normalized;
	}
}

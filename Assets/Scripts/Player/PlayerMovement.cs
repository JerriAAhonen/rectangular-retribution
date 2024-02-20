using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private Transform playerFollowCamera;

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

		var moveHorizontal = Input.GetAxis("Horizontal");
		var moveVertical = Input.GetAxis("Vertical");
		var movement = new Vector3(moveHorizontal, 0f, moveVertical);

		var cameraRotationY = playerFollowCamera.rotation.eulerAngles.y;

		var rotation = Quaternion.Euler(0, cameraRotationY, 0);
		var rotatedMovement = rotation * movement;

		rb.velocity = movementSpeed * Time.deltaTime * rotatedMovement.normalized;
	}
}

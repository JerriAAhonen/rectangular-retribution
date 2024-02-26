using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private PlayerMovement pm;
	private PlayerShooting ps;
	private PlayerExperienceSystem pe;
	
	public PlayerExperienceSystem Experience => pe;

	public bool MovementEnabled { get; private set; }
	public bool ShootingEnabled { get; private set; }

	private void Awake()
	{
		pm = GetComponent<PlayerMovement>();
		pm.Init(this);

		ps = GetComponent<PlayerShooting>();
		ps.Init(this);

		pe = GetComponent<PlayerExperienceSystem>();
		pe.Init(this);
	}

	private void Start()
	{
		MovementEnabled = true;
		ShootingEnabled = true;
	}
}

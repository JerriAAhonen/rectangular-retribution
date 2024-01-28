using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
	[SerializeField] private PlayerController playerController;

	public PlayerController PlayerController => playerController;
}

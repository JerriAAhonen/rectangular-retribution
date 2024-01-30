using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot_Experience : Loot
{
	public override void OnPickup()
	{
		LevelController.Instance.PlayerController.Experience.AddXp(1);
		Destroy(gameObject);
	}
}

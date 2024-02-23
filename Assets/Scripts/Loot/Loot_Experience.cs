using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot_Experience : Loot
{
	protected override void OnDestroyLoot()
	{
		LevelController.Instance.PlayerController.Experience.AddXp(1);
	}
}

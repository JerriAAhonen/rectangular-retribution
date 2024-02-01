using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
	[SerializeField] private LayerMask lootLayer;

	private PlayerController pc;
	private int maxXp = 50;
	private int xp;

	public void Init(PlayerController pc)
	{
		this.pc = pc;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (BitMaskUtil.MaskContainsLayer(lootLayer, other.gameObject.layer))
		{
			if (other.gameObject.TryGetComponent<Loot>(out var loot))
				loot.OnPickup();
		}
	}

	public void AddXp(int amount)
	{
		xp += amount;
		UI_CoreController.Instance.ExperienceView.SetFill(xp / (float)maxXp);
	}
}

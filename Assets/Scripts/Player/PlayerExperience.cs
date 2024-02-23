using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerExperienceEvent : IEvent 
{
	public int currentLevel;
	public int totalXp;
	public float currentLevelXpPercentage;
}

public class PlayerExperienceModel
{
	private PlayerExperienceProgression progression;

	private int currentLevel;
	private int currentLevelXp;
	private int totalXp;
	private float currentLevelXpPercentage;

	public PlayerExperienceModel(PlayerExperienceProgression progression)
	{
		this.progression = progression;
		Reset();
	}

	public void Reset()
	{
		currentLevel = 0;
		currentLevelXp = 0;
		totalXp = 0;
	}

	public bool AddXp(int amount)
	{
		currentLevelXp += amount;
		totalXp += amount;

		var requiredXp = progression.XpNeededForLevel(currentLevel);
		currentLevelXpPercentage = currentLevelXp / (float)requiredXp;

		bool isLevelUp = false;
		if (currentLevelXp >= requiredXp)
		{
			currentLevel++;
			currentLevelXp = 0;
			currentLevelXpPercentage = 0f;
			isLevelUp = true;
		}
		
		EventBus<PlayerExperienceEvent>.Raise(new PlayerExperienceEvent 
		{ 
			currentLevel = currentLevel,
			totalXp = totalXp,
			currentLevelXpPercentage = currentLevelXpPercentage
		});

		return isLevelUp;
	}
}

public class PlayerExperience : MonoBehaviour
{
	[SerializeField] private LayerMask lootLayer;
	[SerializeField] private PlayerExperienceProgression progression;

	private PlayerController pc;
	private PlayerExperienceModel model;

	public void Init(PlayerController pc)
	{
		this.pc = pc;
		model = new PlayerExperienceModel(progression);

		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Level up", InstantLevelUp));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (BitMaskUtil.MaskContainsLayer(lootLayer, other.gameObject.layer))
		{
			if (other.gameObject.TryGetComponent<Loot>(out var loot))
				loot.OnPickup(transform);
		}
	}

	public void AddXp(int amount)
	{
		if (model.AddXp(amount))
		{
			// Level up
			// TODO Move to event bus
			UI_CoreController.Instance.SkillSelection.Open(OnSkillSelectionClose);
		}

		void OnSkillSelectionClose()
		{
			// TODO
		}
	}

	private void InstantLevelUp()
	{
		// TODO
	}
}

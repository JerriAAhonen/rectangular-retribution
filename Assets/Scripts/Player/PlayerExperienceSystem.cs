using UnityEngine;

public struct Event_PlayerGainXp : IEvent 
{
	public int totalXp;
	public float currentLevelXpPercentage;
}

public struct Event_PlayerLevelUp : IEvent
{
	public int newLevel;
}

public class PlayerExperienceSystem : MonoBehaviour
{
	[SerializeField] private LayerMask lootLayer;
	[SerializeField] private PlayerExperienceData progression;

	private PlayerController pc;

	private int currentLevel;
	private int currentLevelXp;
	private int totalXp;
	private float currentLevelXpPercentage;

	public void Init(PlayerController pc)
	{
		this.pc = pc;

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
		if (amount <= 0) return;

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

		EventBus<Event_PlayerGainXp>.Raise(new Event_PlayerGainXp
		{
			totalXp = totalXp,
			currentLevelXpPercentage = currentLevelXpPercentage
		});

		if (isLevelUp)
		{
			EventBus<Event_PlayerLevelUp>.Raise(new Event_PlayerLevelUp
			{
				newLevel = currentLevelXp
			});
		}
	}

	private void InstantLevelUp()
	{
		// TODO
	}
}

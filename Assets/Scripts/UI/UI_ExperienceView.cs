using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExperienceView : MonoBehaviour
{
	[SerializeField] private Image fill;
	[SerializeField] private TextMeshProUGUI totalXpLabel;

	private EventBinding<PlayerExperienceEvent> playerExperienceBinding;

	private void Awake()
	{
		fill.fillAmount = 0f;
	}

	private void OnEnable()
	{
		playerExperienceBinding = new EventBinding<PlayerExperienceEvent>(HandlePlayerExperienceEvent);
		EventBus<PlayerExperienceEvent>.Register(playerExperienceBinding);
	}

	private void OnDisable()
	{
		EventBus<PlayerExperienceEvent>.Deregister(playerExperienceBinding);
	}

	private void HandlePlayerExperienceEvent(PlayerExperienceEvent playerExperienceEvent)
	{
		SetFill(playerExperienceEvent.currentLevelXpPercentage);
		SetTotalXp(playerExperienceEvent.totalXp);
	}

	private void SetFill(float amount01)
	{
		fill.fillAmount = amount01;
	}

	private void SetTotalXp(int totalXp)
	{
		totalXpLabel.text = $"{totalXp}";
	}
}

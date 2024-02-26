using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExperienceView : MonoBehaviour
{
	[SerializeField] private Image fill;
	[SerializeField] private TextMeshProUGUI totalXpLabel;

	private EventBinding<Event_PlayerGainXp> playerGainXpBinding;

	private void Awake()
	{
		fill.fillAmount = 0f;
	}

	private void OnEnable()
	{
		playerGainXpBinding = new EventBinding<Event_PlayerGainXp>(HandlePlayerExperienceEvent);
		EventBus<Event_PlayerGainXp>.Register(playerGainXpBinding);
	}

	private void OnDisable()
	{
		EventBus<Event_PlayerGainXp>.Deregister(playerGainXpBinding);
	}

	private void HandlePlayerExperienceEvent(Event_PlayerGainXp playerExperienceEvent)
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UIEvent_SkillSelectionClosed : IEvent 
{
	public AbilityData chosenAbility;
}

public class UI_SkillSelectionController : MonoBehaviour
{
	[SerializeField] private UI_SkillSelectionView view;

	private List<AbilityData> abilityChoices;

	private EventBinding<Event_PlayerLevelUp> playerLevelUpBinding;

	private void Start()
	{
		view.Init(this);
	}

	private void OnEnable()
	{
		playerLevelUpBinding = new EventBinding<Event_PlayerLevelUp>(HandlePlayerLevelUp);
		EventBus<Event_PlayerLevelUp>.Register(playerLevelUpBinding);
	}

	private void OnDisable()
	{
		EventBus<Event_PlayerLevelUp>.Deregister(playerLevelUpBinding);
	}

	private void HandlePlayerLevelUp(Event_PlayerLevelUp @event)
	{
		Open(@event.abilityChoices);
	}

	private void Open(List<AbilityData> abilityChoices)
	{
		this.abilityChoices = abilityChoices;

		view.Open(abilityChoices);
		TimeController.StopTime();
	}

	public void OnSkillSelected(int index)
	{
		view.Close(index);
		TimeController.ResumeTime();
		EventBus<UIEvent_SkillSelectionClosed>.Raise(new UIEvent_SkillSelectionClosed
		{
			chosenAbility = abilityChoices[index]
		});
	}

	/*
	 ShowSkillSelection
		Randomize options
		Open view
		wait for input
		apply selected skill to player
	 */
}

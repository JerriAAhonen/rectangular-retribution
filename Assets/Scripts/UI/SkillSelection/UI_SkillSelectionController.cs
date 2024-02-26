using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UIEvent_SkillSelectionClosed : IEvent { }

public class UI_SkillSelectionController : MonoBehaviour
{
	[SerializeField] private UI_SkillSelectionView view;

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
		Open();
	}

	private void Open()
	{
		view.Open();
		TimeController.StopTime();
	}

	public void OnSkillSelected(int index)
	{
		view.Close(index);
		TimeController.ResumeTime();
		EventBus<UIEvent_SkillSelectionClosed>.Raise(new UIEvent_SkillSelectionClosed());
	}

	/*
	 ShowSkillSelection
		Randomize options
		Open view
		wait for input
		apply selected skill to player
	 */
}

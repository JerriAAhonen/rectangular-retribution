using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelectionView : MonoBehaviour
{
	private UI_SkillSelectionController controller;
	private List<UI_SkillSelectionViewButton> buttons;

	

	private void Awake()
	{
		buttons = new(GetComponentsInChildren<UI_SkillSelectionViewButton>());

		for (int i = 0; i < buttons.Count; i++)
		{
			//Debug.Log("SkillSelection set listener");
			buttons[i].Init(i);
			buttons[i].Clicked += OnSkill;
		}
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			OnSkill(0);
        }
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			OnSkill(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			OnSkill(2);
		}
	}

	public void Init(UI_SkillSelectionController controller)
	{
		//Debug.Log("SkillSelection init");
		this.controller = controller;
	}

	public void Open()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			foreach (var button in buttons)
			{
				button.Appear();
				yield return CachedWait.ForSeconds(0.2f);
			}
		}
	}

	public void Close()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			foreach (var button in buttons)
			{
				button.Hide();
				yield return CachedWait.ForSeconds(0.2f);
			}
		}
	}

	private void OnSkill(int index)
	{
		Debug.Log($"Skill selected {index}");
		controller.OnSkillSelected(index);
	}

	/*
	 Open
		Stop time
		Show buttons -> Scale animation one by one or staggered
		Send user selection to controller
	 Close
		Opening anim backwards OR some kind on catching kapow flash the selected button
	 */
}

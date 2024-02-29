using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelectionView : MonoBehaviour
{
	private UI_SkillSelectionController controller;
	private List<UI_SkillSelectionViewButton> buttons;

	private CanvasGroup cg;

	private void Awake()
	{
		buttons = new(GetComponentsInChildren<UI_SkillSelectionViewButton>());

		for (int i = 0; i < buttons.Count; i++)
		{
			//Debug.Log("SkillSelection set listener");
			buttons[i].Init(i);
			buttons[i].Clicked += OnSkill;
		}

		cg = GetComponent<CanvasGroup>();
		cg.SetVisible(false);
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

	public void Open(List<AbilityData> abilityChoices)
	{
		cg.SetVisible(true);

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			for (int i = 0;i < buttons.Count;i++)
			{
				buttons[i].SetContent(abilityChoices[i].displayName);
				buttons[i].Appear();
				yield return CachedWait.ForSecondsRealtime(0.2f);
			}
		}
	}

	public void Close(int selectedIndex)
	{
		float maxDur = 0f;
		foreach (var button in buttons)
		{
			var closeDur = button.Hide(selectedIndex);
			if (closeDur > maxDur)
				maxDur = closeDur;
		}

		LeanTween.delayedCall(maxDur, () => { cg.SetVisible(false); });
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

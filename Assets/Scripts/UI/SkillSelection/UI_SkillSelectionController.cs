using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelectionController : Singleton<UI_SkillSelectionController>
{
	[SerializeField] private UI_SkillSelectionView view;

	private void Start()
	{
		view.Init(this);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.L))
		{
			view.Open();
		}
	}

	public void OnSkillSelected(int index)
	{
		view.Close();
	}

	/*
	 ShowSkillSelection
		Randomize options
		Open view
		wait for input
		apply selected skill to player

	 
	 */
}

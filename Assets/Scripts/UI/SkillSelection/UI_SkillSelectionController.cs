using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelectionController : MonoBehaviour
{
	[SerializeField] private UI_SkillSelectionView view;

	private Action onClose;

	private void Start()
	{
		view.Init(this);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.L))
		{
			Open(null);
		}
	}

	public void Open(Action onClose)
	{
		this.onClose = onClose;
		view.Open();
	}

	public void OnSkillSelected(int index)
	{
		onClose?.Invoke();
		onClose = null;
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

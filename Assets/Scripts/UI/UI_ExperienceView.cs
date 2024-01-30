using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExperienceView : MonoBehaviour
{
	[SerializeField] private Image fill;

	private void Awake()
	{
		fill.fillAmount = 0f;
	}

	public void SetFill(float amount01)
	{
		fill.fillAmount = amount01;
	}
}

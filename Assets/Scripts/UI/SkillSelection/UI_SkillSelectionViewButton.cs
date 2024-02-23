using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelectionViewButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TextMeshProUGUI keycodeTooltip;

    private Button button;

    public event Action<int> Clicked;

	public void Init(int index)
    {
		transform.localScale = Vector3.zero;

		button = GetComponent<Button>();
		button.onClick.AddListener(() => Clicked?.Invoke(index));

		label.text = $"Skill {index}";
		keycodeTooltip.text = $"{index + 1}";
    }

	public void Appear()
	{
		LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
	}

	public void Hide()
	{
		LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInBack();
	}
}

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
	[Header("Animations")]
    [SerializeField] private float appearDur = 0.5f;
    [SerializeField] private float hideDur = 0.5f;
	[Header("Select Animation")]
    [SerializeField] private float scaleUpDur = 0.5f;
    [SerializeField] private float scaleUpSize = 1.3f;
    [SerializeField] private float scaleDownDur = 0.3f;

    private Button button;
	private int index;

    public event Action<int> Clicked;

	public void Init(int index)
    {
		this.index = index;
		transform.localScale = Vector3.zero;

		button = GetComponent<Button>();
		button.onClick.AddListener(Select);

		label.text = $"Skill {index}";
		keycodeTooltip.text = $"{index + 1}";
    }

	public void Select()
	{
		Clicked?.Invoke(index);
	}

	public void Appear()
	{
		LeanTween.scale(gameObject, Vector3.one, appearDur).setEaseOutBack().setIgnoreTimeScale(true);
	}

	public float Hide(int selectedIndex)
	{
		if (selectedIndex ==  index)
		{
			var seq = LeanTween.sequence(gameObject);
			seq.append(LeanTween.scale(gameObject, Vector3.one * scaleUpSize, scaleUpDur).setEaseOutExpo().setIgnoreTimeScale(true));
			seq.append(LeanTween.scale(gameObject, Vector3.zero, scaleDownDur).setEaseInBack().setIgnoreTimeScale(true));
			return scaleUpDur + scaleDownDur;
		}
		else
		{
			LeanTween.scale(gameObject, Vector3.zero, hideDur).setEaseInBack().setIgnoreTimeScale(true);
			return hideDur;
		}
	}
}

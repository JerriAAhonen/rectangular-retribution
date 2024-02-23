using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CoreController : Singleton<UI_CoreController>
{
	[SerializeField] private UI_SkillSelectionController skillSelection;

	public UI_SkillSelectionController SkillSelection => skillSelection;
}

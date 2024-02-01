using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CoreController : Singleton<UI_CoreController>
{
	[SerializeField] private UI_ExperienceView experienceView;

	public UI_ExperienceView ExperienceView => experienceView;
}

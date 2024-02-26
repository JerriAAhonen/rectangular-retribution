using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public interface IAbilityData { }

public class AbilityData : ScriptableObject
{
	[ReadOnly] public int id;
	public string displayName;

	public List<GameObject> prefabsToAdd;
	public List<object> bonusesToApply;
}

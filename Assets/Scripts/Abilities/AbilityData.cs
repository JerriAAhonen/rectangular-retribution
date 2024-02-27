using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class AbilityData : ScriptableObject
{
	[ReadOnly] public int id;
	public string displayName;

	public List<IAbilityComponent> components;
}

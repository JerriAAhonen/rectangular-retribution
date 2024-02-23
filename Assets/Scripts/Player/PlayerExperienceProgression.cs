using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerExperienceProgression : ScriptableObject
{
	[SerializeField] private List<int> levelXpRequirements;

	public List<int> LevelXpRequirements => levelXpRequirements;

	public int XpNeededForLevel(int level)
	{
		if (level < 0 || level > levelXpRequirements.Count - 1)
			return levelXpRequirements[^1];
		return levelXpRequirements[level];
	}
}

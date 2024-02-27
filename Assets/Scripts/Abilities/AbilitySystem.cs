using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityModel
{
	private readonly List<AbilityData> allAbilities;

	public int ProjectileCount { get; private set; }
	public int ProjectileDamageMult { get; private set; }
	public int ProjectileSpeedMult { get; private set; }
	public int ProjectileFireSpeedMult { get; private set; }

	public AbilityModel(AbilityDatabase abilityDatabase) 
	{
		allAbilities = new(abilityDatabase.allAbilities);
	}

	public AbilityData GetRandomAbility(List<AbilityData> exclude)
	{
		AbilityData randAbilityData;
		do
		{
			var randIndex = Random.Range(0, allAbilities.Count - 1);
			randAbilityData = allAbilities[randIndex];

		} while (exclude.Contains(randAbilityData));

		return randAbilityData;
	}
}

public class AbilityController
{
	private readonly AbilityModel model;

	public AbilityController(AbilityModel model)
	{
		this.model = model;
	}

	public void GetAbilityChoisesOnLevelUp(int amount, List<AbilityData> result)
	{
		result.Clear();
		for (int i = 0; i < amount; i++)
		{
			result.Add(model.GetRandomAbility(result));
		}
	}

	public void OnNewAbilityChosen(AbilityData data)
	{

	}
}

public class AbilitySystem : MonoBehaviour
{
	[SerializeField] private AbilityDatabase abilityDatabase;

	private AbilityModel model;
	private AbilityController controller;

	private void Awake()
	{
		model = new AbilityModel(abilityDatabase);
		controller = new AbilityController(model);
	}

	public void GetAbilityChoisesOnLevelUp(int amount, List<AbilityData> result) => controller.GetAbilityChoisesOnLevelUp(amount, result);
}

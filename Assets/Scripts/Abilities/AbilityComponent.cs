using UnityEngine;

public interface IAbilityComponent
{
	void Activate();
	void Deactivate();
}

public class AbilityComponent : MonoBehaviour, IAbilityComponent
{
	public void Activate()
	{
	}

	public void Deactivate()
	{
	}
}

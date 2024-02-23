using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnArea_Circle : ISpawnArea
{
	private Vector3 origin;
	private float innerRadius;
	private float outerRadius;

	public SpawnArea_Circle(Vector3 origin, float innerRadius, float outerRadius) 
	{
		this.origin = origin;
		this.innerRadius = innerRadius;
		this.outerRadius = outerRadius;
	}

	public Vector3 GetRandomPosition()
	{
		var randDistance = Random.Range(innerRadius, outerRadius);
		var randUnitCircle = Random.insideUnitCircle.normalized;
		var randDir = randUnitCircle * randDistance;
		return new Vector3(randDir.x, 0f, randDir.y);
	}

	public void DrawGizmos(Vector3 offsetToOrigin)
	{
		Handles.DrawWireDisc(origin + offsetToOrigin, Vector3.up, innerRadius);
		Handles.DrawWireDisc(origin + offsetToOrigin, Vector3.up, outerRadius);
	}

}

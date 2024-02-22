using UnityEngine;

public interface ISpawnArea
{
	Vector3 GetRandomPosition();
	void DrawGizmos(Vector3 offsetToOrigin);
}

using UnityEngine;

public class SpawnArea_Rectangular : ISpawnArea
{
	public SpawnSquare left; 
	public SpawnSquare top; 
	public SpawnSquare right; 
	public SpawnSquare bot;

	public SpawnArea_Rectangular(Vector3 playerPos, float spawnAreaDepth, float spawnAreaOffset)
	{
		var cameraBotLeftCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.zero).With(y: 0f);
		var cameraTopRightCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.one).With(y: 0f);
		var cameraTopLeftCorner = new Vector3(cameraBotLeftCorner.x, cameraBotLeftCorner.y, cameraTopRightCorner.z).With(y: 0f);
		var cameraBotRightCorner = new Vector3(cameraTopRightCorner.x, cameraTopRightCorner.y, cameraBotLeftCorner.z).With(y: 0f);

		var botLeftOffset = cameraBotLeftCorner - playerPos;
		var botRightOffset = cameraBotRightCorner - playerPos;
		var topLeftOffset = cameraTopLeftCorner - playerPos;
		var topRightOffset = cameraTopRightCorner - playerPos;

		Vector3 camBotLeft = cameraBotLeftCorner + (botLeftOffset).normalized * spawnAreaOffset;
		Vector3 camTopLeft = cameraTopLeftCorner + (topLeftOffset).normalized * spawnAreaOffset;
		Vector3 camTopRight = cameraTopRightCorner + (topRightOffset).normalized * spawnAreaOffset;
		Vector3 camBotRight = cameraBotRightCorner + (botRightOffset).normalized * spawnAreaOffset;
		

		CalculateLeft(camBotLeft, camTopLeft);
		CalculateTop(camTopLeft, camTopRight);
		CalculateRight(camTopRight, camBotRight);
		CalculateBot(camBotLeft, camBotRight);

		void CalculateLeft(Vector3 camBotLeft, Vector3 camTopLeft)
		{
			var botLeft = camBotLeft + Vector3.back * spawnAreaDepth + Vector3.left * spawnAreaDepth;
			var topRight = camTopLeft + Vector3.forward * spawnAreaDepth;
			left = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateTop(Vector3 camTopLeft, Vector3 camTopRight)
		{
			var botLeft = camTopLeft;
			var topRight = camTopRight + Vector3.forward * spawnAreaDepth;
			top = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateRight(Vector3 camTopRight, Vector3 camBotRight)
		{
			var botLeft = camBotRight + Vector3.back * spawnAreaDepth;
			var topRight = camTopRight + Vector3.forward * spawnAreaDepth + Vector3.right * spawnAreaDepth;
			right = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateBot(Vector3 camBotLeft, Vector3 camBotRight)
		{
			var botLeft = camBotLeft + Vector3.back * spawnAreaDepth;
			var topRight = camBotRight;
			bot = CreateSpawnSquare(botLeft, topRight);
		}

		SpawnSquare CreateSpawnSquare(Vector3 botLeft, Vector3 topRight)
		{
			var center = GetCenter(botLeft, topRight);
			var size = GetSize(botLeft, topRight);
			return new SpawnSquare(center, size);
		}

		Vector3 GetCenter(Vector3 botLeft, Vector3 topRight) => (botLeft + topRight) / 2f;
		Vector3 GetSize(Vector3 botLeft, Vector3 topRight) => topRight - botLeft;
	}

	public Vector3 GetRandomPosition()
	{
		int side = Random.Range(0, 4);
		return side switch
		{
			0 => right.GetRandomPosition(),
			1 => top.GetRandomPosition(),
			2 => left.GetRandomPosition(),
			3 => bot.GetRandomPosition(),
			_ => throw new System.NotImplementedException()
		};
	}

	public void DrawGizmos(Vector3 offsetToOrigin)
	{
		left.DrawGizmos(offsetToOrigin);
		top.DrawGizmos(offsetToOrigin);
		right.DrawGizmos(offsetToOrigin);
		bot.DrawGizmos(offsetToOrigin);
	}
}

public class SpawnSquare
{
	Vector3 center;
	Vector3 size;
	Vector3 half;

	public SpawnSquare(Vector3 center, Vector3 size)
	{
		this.center = center;
		this.size = size;
		half = size / 2f;
	}

	public Vector3 GetRandomPosition()
	{
		Vector3 randomPoint = new(
			Random.Range(-half.x, half.x),
			Random.Range(-half.y, half.y),
			Random.Range(-half.z, half.z)
		);

		// Add the center of the box to the random point to get a point within the box
		return center + randomPoint;
	}

	public void DrawGizmos(Vector3 offsetToOrigin)
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(center + offsetToOrigin, 0.2f);
		Gizmos.DrawWireCube(center + offsetToOrigin, size);
	}
}

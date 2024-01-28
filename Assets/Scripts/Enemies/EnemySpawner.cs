using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private Enemy enemyPrefab;
	[SerializeField] private float spawnAreaDepth;
	[SerializeField] private float spawnAreaOffset;

	private Vector3 cameraBotLeftCorner; // (0,0)
	private Vector3 cameraTopRightCorner;// (1,1)
	private Vector3 cameraTopLeftCorner; // (0,1)
	private Vector3 cameraBotRightCorner;// (1,0)

	private Vector3 botLeftOffset;
	private Vector3 botRightOffset;
	private Vector3 topLeftOffset;
	private Vector3 topRightOffset;

	private Vector3 origin;
	private SpawnArea spawnArea;

	private Vector3 randomizedPosition;


	private void Start()
	{
		cameraBotLeftCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.zero).With(y: 0f);
		cameraTopRightCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.one).With(y: 0f);
		cameraTopLeftCorner = new Vector3(cameraBotLeftCorner.x, cameraBotLeftCorner.y, cameraTopRightCorner.z).With(y: 0f);
		cameraBotRightCorner = new Vector3(cameraTopRightCorner.x, cameraTopRightCorner.y, cameraBotLeftCorner.z).With(y: 0f);

		botLeftOffset = cameraBotLeftCorner - transform.position.With(y: 0f);
		botRightOffset = cameraBotRightCorner - transform.position.With(y: 0f);
		topLeftOffset = cameraTopLeftCorner - transform.position.With(y: 0f);
		topRightOffset = cameraTopRightCorner - transform.position.With(y: 0f);

		origin = transform.position.With(y: 0f);

		spawnArea = new(
			cameraBotLeftCorner + (botLeftOffset).normalized * spawnAreaOffset,
			cameraTopLeftCorner + (topLeftOffset).normalized * spawnAreaOffset,
			cameraTopRightCorner + (topRightOffset).normalized * spawnAreaOffset,
			cameraBotRightCorner + (botRightOffset).normalized * spawnAreaOffset,
			spawnAreaDepth);

		randomizedPosition = spawnArea.GetRandomPosition();

		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Spawn Wave", SpawnWave));
	}

	/// <summary>
	/// - Find corners of the camera view
	/// - Add a little padding so you dont see them spawning in
	/// - randomize position along the edges of the screen
	/// - assert that enemies are evenly spread (atleast for a basic wave)
	/// </summary>
	private void SpawnWave()
	{
		var enemy = Instantiate(enemyPrefab);
		enemy.transform.position = spawnArea.GetRandomPosition();

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = spawnArea.GetRandomPosition();

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = spawnArea.GetRandomPosition();

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = spawnArea.GetRandomPosition();
	}

	private void OnDrawGizmos()
	{
		spawnArea?.DrawGizmos();

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(randomizedPosition, 0.2f);
	}
}

public class SpawnArea
{
	public SpawnSquare left; 
	public SpawnSquare top; 
	public SpawnSquare right; 
	public SpawnSquare bot;

	public SpawnArea(Vector3 camBotLeft,  Vector3 camTopLeft, Vector3 camTopRight, Vector3 camBotRight, float depth)
	{
		CalculateLeft(camBotLeft, camTopLeft);
		CalculateTop(camTopLeft, camTopRight);
		CalculateRight(camTopRight, camBotRight);
		CalculateBot(camBotLeft, camBotRight);

		void CalculateLeft(Vector3 camBotLeft, Vector3 camTopLeft)
		{
			var botLeft = camBotLeft + Vector3.back * depth + Vector3.left * depth;
			var topRight = camTopLeft + Vector3.forward * depth;
			left = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateTop(Vector3 camTopLeft, Vector3 camTopRight)
		{
			var botLeft = camTopLeft;
			var topRight = camTopRight + Vector3.forward * depth;
			top = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateRight(Vector3 camTopRight, Vector3 camBotRight)
		{
			var botLeft = camBotRight + Vector3.back * depth;
			var topRight = camTopRight + Vector3.forward * depth + Vector3.right * depth;
			right = CreateSpawnSquare(botLeft, topRight);
		}

		void CalculateBot(Vector3 camBotLeft, Vector3 camBotRight)
		{
			var botLeft = camBotLeft + Vector3.back * depth;
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

	public void DrawGizmos()
	{
		left.DrawGizmos();
		top.DrawGizmos();
		right.DrawGizmos();
		bot.DrawGizmos();
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

	public void DrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(center, 0.2f);
		Gizmos.DrawWireCube(center, size);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	/// <summary>
	/// Wave duration in seconds
	/// </summary>
	public const float WaveDuration = 60f;

	[SerializeField] private Enemy enemyPrefab;
	[SerializeField] private int enemiesPerWave;
	[SerializeField] private int enemiesPerWaveIncreasePerWave;
	[SerializeField] private float spawnAreaDepth;
	[SerializeField] private float spawnAreaOffset;
	[Space]
	[SerializeField] private bool debug_Enable;

	private Vector3 cameraBotLeftCorner; // (0,0)
	private Vector3 cameraTopRightCorner;// (1,1)
	private Vector3 cameraTopLeftCorner; // (0,1)
	private Vector3 cameraBotRightCorner;// (1,0)

	private Vector3 origin;
	private SpawnArea spawnArea_outside;

	private readonly HashSet<Enemy> spawnedEnemies = new();
	private CountdownTimer waveTimer;
	private DebugElement_Label debugElement_waveTimer;
	private DebugElement_Label debugElement_waveNumber;

	private int curWave;
	private int curEnemiesPerWave;

	private void Start()
	{
		InitializeSpawnArea();

		waveTimer = new(WaveDuration);
		waveTimer.OnTimerStop += SpawnWave;
		waveTimer.Start();

		curWave = -1;
		curEnemiesPerWave = enemiesPerWave - enemiesPerWaveIncreasePerWave;

		SpawnWave();
		InitializeDebugElements();
	}

	private void Update()
	{
		waveTimer.Tick(Time.deltaTime);
		debugElement_waveTimer.SetText($"Next wave in: {waveTimer.Time}s");
	}

	private void InitializeDebugElements()
	{
		debugElement_waveTimer = new($"Next wave in: {waveTimer.Time}s");
		debugElement_waveNumber = new($"Current Wave: {curWave}");

		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Space());
		DebugMenu.Instance.RegisterDebugElement(debugElement_waveTimer);
		DebugMenu.Instance.RegisterDebugElement(debugElement_waveNumber);
		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Spawn Wave", SpawnWave));
	}

	private void InitializeSpawnArea()
	{
		cameraBotLeftCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.zero).With(y: 0f);
		cameraTopRightCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.one).With(y: 0f);
		cameraTopLeftCorner = new Vector3(cameraBotLeftCorner.x, cameraBotLeftCorner.y, cameraTopRightCorner.z).With(y: 0f);
		cameraBotRightCorner = new Vector3(cameraTopRightCorner.x, cameraTopRightCorner.y, cameraBotLeftCorner.z).With(y: 0f);

		var botLeftOffset = cameraBotLeftCorner - transform.position.With(y: 0f);
		var botRightOffset = cameraBotRightCorner - transform.position.With(y: 0f);
		var topLeftOffset = cameraTopLeftCorner - transform.position.With(y: 0f);
		var topRightOffset = cameraTopRightCorner - transform.position.With(y: 0f);

		origin = transform.position.With(y: 0f);

		spawnArea_outside = new(
			cameraBotLeftCorner + (botLeftOffset).normalized * spawnAreaOffset,
			cameraTopLeftCorner + (topLeftOffset).normalized * spawnAreaOffset,
			cameraTopRightCorner + (topRightOffset).normalized * spawnAreaOffset,
			cameraBotRightCorner + (botRightOffset).normalized * spawnAreaOffset,
			spawnAreaDepth);
	}

	/// <summary>
	/// - Find corners of the camera view
	/// - Add a little padding so you dont see them spawning in
	/// - randomize position along the edges of the screen
	/// - assert that enemies are evenly spread (atleast for a basic wave)
	/// </summary>
	private void SpawnWave()
	{
		curWave++;
		curEnemiesPerWave += enemiesPerWaveIncreasePerWave;
		StartCoroutine(Routine());

		debugElement_waveNumber?.SetText($"Current Wave: {curWave}");

		IEnumerator Routine()
		{
			for (int i = 0; i < curEnemiesPerWave; i++)
			{
				var enemy = Instantiate(enemyPrefab);
				enemy.transform.position = spawnArea_outside.GetRandomPosition() + GetOffsetToOrigin();
				enemy.Init(OnEnemyDied);
				spawnedEnemies.Add(enemy);

				yield return null;
			}

			waveTimer.Start();
		}
	}

	private void OnDrawGizmos()
	{
		if (!debug_Enable) return;
		spawnArea_outside?.DrawGizmos(GetOffsetToOrigin());
	}

	private void OnEnemyDied(Enemy enemy)
	{
		spawnedEnemies.Remove(enemy);
		if (spawnedEnemies.Count == 0)
			SpawnWave();
	}

	private Vector3 GetOffsetToOrigin() => transform.position.With(y: 0f) - origin;
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

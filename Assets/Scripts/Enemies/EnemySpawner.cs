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

	private Vector3 origin;
	private ISpawnArea spawnArea;

	private bool doSpawn;
	private readonly HashSet<Enemy> spawnedEnemies = new();
	private CountdownTimer waveTimer;

	private DebugElement_Label debugElement_waveTimer;
	private DebugElement_Label debugElement_waveNumber;

	private int curWave;
	private int curEnemiesPerWave;

	private void Start()
	{
		InitializeSpawnArea();

		doSpawn = true;
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

	private void InitializeSpawnArea()
	{
		origin = transform.position.With(y: 0f);
		//spawnArea = new SpawnArea_Rectangular(transform.position.With(y: 0f), spawnAreaDepth, spawnAreaOffset);
		spawnArea = new SpawnArea_Circle(origin, spawnAreaOffset, spawnAreaOffset + spawnAreaDepth);
	}

	private void InitializeDebugElements()
	{
		debugElement_waveTimer = new($"Next wave in: {waveTimer.Time}s");
		debugElement_waveNumber = new($"Current Wave: {curWave}");

		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Space());
		DebugMenu.Instance.RegisterDebugElement(debugElement_waveTimer);
		DebugMenu.Instance.RegisterDebugElement(debugElement_waveNumber);
		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Spawn Wave", SpawnWave));
		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Clear enemies", ClearEnemies));
		DebugMenu.Instance.RegisterDebugElement(new DebugElement_Button("Toggle spawning", ToggleDoSpawn)); // TODO Display if spawning is on or off
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
				enemy.transform.position = spawnArea.GetRandomPosition() + GetOffsetToOrigin();
				enemy.Init(OnEnemyDied);
				spawnedEnemies.Add(enemy);

				yield return null;
			}

			waveTimer.Start();
		}
	}

	private void ClearEnemies()
	{
		List<Enemy> enemiesToClear = new List<Enemy>(spawnedEnemies);

		foreach (var enemy in enemiesToClear)
		{
			enemy.TakeDamage(9999);
		}
	}

	private void ToggleDoSpawn() => doSpawn = !doSpawn;

	private void OnDrawGizmos()
	{
		if (!debug_Enable) return;
		spawnArea?.DrawGizmos(GetOffsetToOrigin());
	}

	private void OnEnemyDied(Enemy enemy)
	{
		spawnedEnemies.Remove(enemy);
		if (spawnedEnemies.Count == 0)
			SpawnWave();
	}

	private Vector3 GetOffsetToOrigin() => transform.position.With(y: 0f) - origin;
}

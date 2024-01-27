using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private Enemy enemyPrefab;

	/// <summary>
	/// - Find corners of the camera view
	/// - Add a little padding so you dont see them spawning in
	/// - randomize position along the edges of the screen
	/// - assert that enemies are evenly spread (atleast for a basic wave)
	/// </summary>
	private void SpawnWave()
	{
		var cameraBotLeftCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.zero);
		var cameraTopRightCorner = CameraUtil.GetPositionOnCameraEdge(Camera.main, Vector2Int.one);

		var cameraTopLeftCorner = new Vector3(cameraBotLeftCorner.x, cameraBotLeftCorner.y, cameraTopRightCorner.z);
		var cameraBotRightCorner = new Vector3(cameraTopRightCorner.x, cameraTopRightCorner.y, cameraBotLeftCorner.z);

		var enemy = Instantiate(enemyPrefab);
		enemy.transform.position = cameraTopLeftCorner;

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = cameraTopRightCorner;

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = cameraBotLeftCorner;

		enemy = Instantiate(enemyPrefab);
		enemy.transform.position = cameraBotRightCorner;
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Spawn wave"))
		{
			SpawnWave();
		}
	}
}

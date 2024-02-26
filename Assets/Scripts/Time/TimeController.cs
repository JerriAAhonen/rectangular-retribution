using UnityEngine;

public static class TimeController
{
	public static void SetTimeScale(float scale)
	{
		Time.timeScale = scale;
	}

	public static void StopTime()
	{
		Time.timeScale = 0f;
	}

	public static void ResumeTime()
	{
		Time.timeScale = 1f;
	}
}

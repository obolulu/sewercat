using UnityEngine;

public static class TimescaleManager
{
    private static float originalFixedDeltaTime = Time.fixedDeltaTime;

    public static void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
    }
    
    public static void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
}
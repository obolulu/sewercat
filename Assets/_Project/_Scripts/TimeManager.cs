using UnityEngine;

public static class TimeManager
{
    private static float originalFixedDeltaTime = Time.fixedDeltaTime;
    
    public static float TimeScale => Time.timeScale;
    
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
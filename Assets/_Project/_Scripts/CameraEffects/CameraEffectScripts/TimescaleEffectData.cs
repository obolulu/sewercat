using System.Collections;
using _Project._Scripts.CameraEffects;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera Effects/Timescale")]

public class TimeScaleEffectData : CameraEffectData
{
    public float timeScale = 0.2f;

    public override IEnumerator Execute(Camera camera)
    {
        TimeManager.SetTimeScale(timeScale);
        yield return new WaitForSecondsRealtime(duration);
        TimeManager.ResetTimeScale();
    }
}
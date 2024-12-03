/*
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FMODEvents", menuName = "Audio/FMODEvents")]
public class FMODEvents : ScriptableObject
{
    [Header("SFX")]
    [SerializeField] private EventReference playerFootsteps;
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerLand;
    
    [Header("Music")]
    [SerializeField] private EventReference backgroundMusic;
    [SerializeField] private EventReference combatMusic;
    
    [Header("Ambience")]
    [SerializeField] private EventReference environmentAmbience;
    
    public EventReference PlayerFootsteps     => playerFootsteps;
    public EventReference PlayerJump          => playerJump;
    public EventReference PlayerLand          => playerLand;
    public EventReference BackgroundMusic     => backgroundMusic;
    public EventReference CombatMusic         => combatMusic;
    public EventReference EnvironmentAmbience => environmentAmbience;
    
    private static FMODEvents instance;
    public static FMODEvents Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<FMODEvents>("FMODEvents");
            return instance;
        }
    }
}
*/
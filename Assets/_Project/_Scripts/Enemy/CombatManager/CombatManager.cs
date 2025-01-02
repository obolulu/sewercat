using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Enemy;
using _Project._Scripts.PlayerScripts;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerController player;
    public static CombatManager Instance;
    private List<Enemy1> enemyList = new();
    
    //public fields
    public Vector3 PlayerPos => player.PlayerPosition;
    private void Awake()
    {
        if(CombatManager.Instance == null)
        {
            CombatManager.Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    //is called in FixedUpdate for now, will change in the future to tick after a while like 0.2 or 0.5 seconds
    private void FixedUpdate()
    {
        CustomUpdate();
    }

    //will make decisions about the new combat goal (aggressive, defensive, flee etc.)
    private void CustomUpdate()
    {
        CheckAllEngagement();
    }
    
    public void RegisterEnemy(Enemy1 enemy)
    {
        enemyList.Add(enemy);
    }
    
    public void UnregisterEnemy(Enemy1 enemy)
    {
        enemyList.Remove(enemy);
    }

    private void CheckAllEngagement()
    {
        foreach (var enemy in enemyList)
        {
            CheckEngagement(enemy);
        }

        return;

        void CheckEngagement(Enemy1 enemy)
        {
            if(enemy.ShouldDisengage)
            {
                enemy.Disengage();
            }
            else
            {
                enemy.Engage();
            }
        }
    }

}
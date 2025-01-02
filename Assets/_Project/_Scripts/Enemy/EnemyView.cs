using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private GameObject viewCollidersParent;
    private List<Collider> viewColliders;
    
    public event Action OnEngage;
    
    public void DisableColliders()
    {
        foreach (var collider in viewColliders)
        {
            collider.enabled = false;
        }
    }

    public void EnableColliders()
    {
        foreach (var collider in viewColliders)
        {
            collider.enabled = true;
        }
    }
    private void Awake()
    {
        RegisterColliders();
    }
    
    private void RegisterColliders()
    {
        if(viewColliders == null)
        {
            viewColliders = new List<Collider>();
        }
        viewColliders.Clear();
        foreach (var collider in viewCollidersParent.transform.GetComponents<Collider>())
        {
            viewColliders.Add(collider);
        }
        Debug.Log(viewColliders);
    }
    private void OnTriggerEnter(Collider other)
    {
            //Engage();
            OnEngage?.Invoke();
    }
}
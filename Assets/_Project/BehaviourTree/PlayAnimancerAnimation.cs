using System;
using Animancer;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace _Project.BehaviourTree
{
    [Category("00 Animations")]
public class PlayAnimancerAnimation : ActionTask
{
    public AnimationClip Clip;
    private AnimancerComponent _animancerComponent;
    private AnimancerState _currentState;
    private bool _hasStarted;

    protected override string OnInit()
    {
        _animancerComponent = agent.GetComponent<AnimancerComponent>();
        return _animancerComponent == null ? "No AnimancerComponent found!" : null;
    }

    protected override void OnExecute()
    {
        if (!_hasStarted)
        {
            StartNewAnimation();
        }
    }

    private void StartNewAnimation()
    {
        Debug.LogError("Starting new animation");
        _currentState = _animancerComponent.Play(Clip);
        _currentState.Events(this).OnEnd = HandleAnimationEnd;
        _hasStarted = true;
    }

    private void HandleAnimationEnd()
    {
        EndAction(true);
    }

    protected override void OnStop()
    {
        if (_currentState != null)
        {
            _currentState.Events(this).OnEnd = null;
        }
        _hasStarted = false;
        base.OnStop();
    }
}
}
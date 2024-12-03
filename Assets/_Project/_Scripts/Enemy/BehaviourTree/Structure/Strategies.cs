using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.BehaviourTree.Structure
{
    public interface IStrategy
    {
        Node.NodeState Evaluate();
        void Reset() {}
    }
    
    public class ActionStrategy : IStrategy
    {
        private readonly Action _doAction;
        
        public ActionStrategy(Action action)
        {
            this._doAction = action;
        }
        
        public Node.NodeState Evaluate()
        {
            _doAction();
            return Node.NodeState.Success;
        }
    }
    
    
    public class Condition: IStrategy
    {
        private readonly Func<bool> _predicate;
        
        public Condition(System.Func<bool> condition)
        {
            this._predicate = condition;
        }
        
        public Node.NodeState Evaluate()
        {
            return _predicate() ? Node.NodeState.Success : Node.NodeState.Failure;
        }
    }
    public class PatrolStrategy : IStrategy
    {
        readonly Transform       _entity;
        readonly NavMeshAgent    _agent;
        readonly List<Transform> _patrolPoints;
        readonly float           _patrolSpeed;
        private  int             _currentIndex;
        private  bool            _isPathCalculated;
        
        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this._entity       = entity;
            this._agent        = agent;
            this._patrolPoints = patrolPoints;
            this._patrolSpeed   = patrolSpeed;
        }
        
        public Node.NodeState Evaluate()
        {
            //if(_currentIndex == _patrolPoints.Count) return Node.NodeState.Success;
            if(_currentIndex == _patrolPoints.Count) _currentIndex = 0;
            
            var target = _patrolPoints[_currentIndex];
            _agent.SetDestination(target.position);
            _entity.LookAt(target);
            if(_isPathCalculated && _agent.remainingDistance <= 0.1f)
            {
                _currentIndex++;
                _isPathCalculated = false;
            }
            if(_agent.pathPending)
            {
                _isPathCalculated = true;
            }
            return Node.NodeState.Running;
        }

        public void Reset() => _currentIndex = 0;
    }
    
    
}
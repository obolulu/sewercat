using System;
using System.Collections.Generic;
using System.Linq;

namespace _Project._Scripts.EnemyDir.BehaviourTree.Structure
{
    public class Parallel : Node
    {
        public enum Policy
        {
            RequireOne, // Success if one child succeeds, fail if all fail
            RequireAll, // Success if all children succeed, fail if one fails
            RequireNone // Always returns Running to let all children execute
        }
        
        private readonly Policy _successPolicy;
        private readonly Policy _failurePolicy;
        
        public Parallel(string name, Policy successPolicy = Policy.RequireOne, Policy failurePolicy = Policy.RequireOne) : base(name)
        {
            _successPolicy = successPolicy;
            _failurePolicy = failurePolicy;
        }

        public override NodeState Evaluate()
        {
            int successCount = 0;
            int failureCount = 0;
            
            foreach (var child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Success:
                        successCount++;
                        break;
                    case NodeState.Failure:
                        failureCount++;
                        break;
                }
            }
            if (successCount == children.Count && _successPolicy == Policy.RequireAll)
                return NodeState.Success;
            
            if (failureCount == children.Count && _failurePolicy == Policy.RequireAll)
                return NodeState.Failure;
            
            return NodeState.Running;
        }
        
    }
    public class UntilFail: Node
    {
        public UntilFail(string name) : base(name) { }

        public override NodeState Evaluate()
        {
            if(children[0].Evaluate() == NodeState.Failure)
            {
                return NodeState.Failure;
            }

            return NodeState.Running;
        }
    }
    
    public class Inverter : Node
    {
        public Inverter(string name) : base(name) { }
        
        public override NodeState Evaluate()
        {
            switch (children[0].Evaluate())
            {
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Success:
                    return NodeState.Failure;
                default:
                    return NodeState.Running;
            }
        }
    }

    public class RandomSelector: PrioritySelector
    {
        protected override List<Node> SortChildren() => children.Shuffle().ToList();
        public RandomSelector(string name) : base(name) { }
    }
    
    public class PrioritySelector : Selector
    {
        List<Node>   sortedChildren;
        List<Node>   SortedChildren => sortedChildren ??= SortChildren();

        protected virtual List<Node> SortChildren() => children.OrderByDescending(child => child.priority).ToList();
        public PrioritySelector(string name) : base(name) { }
        public override void Reset()
        {
            base.Reset();
            sortedChildren = null;
        }

        public override NodeState Evaluate()
        {
            foreach (var child in SortedChildren)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        Reset();
                        return NodeState.Success;
                    default:
                        continue;
                }
            }

            return NodeState.Failure;
        }
    }

    public class Selector : Node
    {
        public Selector(string name, int priority = 0) : base(name, priority) { }

        public override NodeState Evaluate()
        {
            if(currentChild < children.Count)
            {
                switch (children[currentChild].Evaluate())
                {
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        Reset();
                        return NodeState.Success;
                    default:
                        currentChild++;
                        return NodeState.Running;
                }
            }
            Reset();
            return NodeState.Failure;
        }
    }
    
    
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority) { }

        public override NodeState Evaluate()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Evaluate())
                {
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Failure:
                        Reset();
                        return NodeState.Failure;
                    default:
                        currentChild++;
                        return currentChild == children.Count ? NodeState.Success : NodeState.Running;
                }
            }
            Reset();
            return NodeState.Success;
        }
    }
    public class Leaf : Node
    {

        private IStrategy _strategies;
        public Leaf(string name, IStrategy strategies, int priority = 0): base(name,priority)
        {
            _strategies = strategies;
        }

        public override NodeState Evaluate() => _strategies.Evaluate();
    }
    public class Node
    {

        public enum NodeState { Success, Failure, Running }

        public string name;
        public int priority;
        
        public readonly List<Node> children = new List<Node>();
        protected       int        currentChild;
        public          NodeState  State { get; protected set; }
        
        public Node(string name, int priority = 0)
        {
            this.name     = name;
            this.priority = priority;
        }

        public void AddChild(Node child) => children.Add(child);
        public virtual NodeState Evaluate() => children[currentChild].Evaluate();

        public virtual void Reset()
        {
            currentChild = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
        }
        
    }
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name, int priority = 0) : base(name, priority) { }

        public override NodeState Evaluate()
        {
            while(currentChild < children.Count )
            {
                var state = children[0].Evaluate();
                if (state != NodeState.Success)
                {
                    return state;
                }
            }
            return NodeState.Success;
        }
    }
    
    static class MyExtensions
    {
        private static readonly Random random = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k     = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
            return list;
        }
    }
}

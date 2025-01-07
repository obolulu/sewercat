using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Weapons.Claws.States.StateDatas
{
    [CreateAssetMenu(fileName = "BlockStateData", menuName = "State Data/Claws/Block State Data")]
    public class BlockStateData: StateData
    {
        [Title("Animations & Effects")]
        public AnimationClip blockAnimation;
        public AnimationClip blockEndAnimation;
        public AnimationClip blockIdleAnimation;
    }
}
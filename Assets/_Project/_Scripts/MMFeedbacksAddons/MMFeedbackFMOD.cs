using MoreMountains.Feedbacks;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
    [AddComponentMenu("")]
    [FeedbackHelp("Plays a sound through the AudioManager")]
    [FeedbackPath("Audio/FMOD Sound")]
    public class MMFeedbackFMOD : MMF_Feedback
    {
        public float FeedbackDuration
        {
            get => 0f;
        }
        [MMFInspectorGroup("SoundInfo", true, 57)]
        [Header("Sound Event")] 
        [SerializeField] private FMODEventSO eventSO;

        [Header("Position Settings")]       public bool usePosition      = true;
        [MMFCondition("usePosition", true)] public bool useOwnerPosition = true;

        [MMFCondition("useOwnerPosition", false)]
        public Vector3 specificPosition;

        [Header("Parameters")] public AudioManagerParameter[] parameters;

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active || eventSO == null) return;

            Vector3? soundPosition = null;
            if (usePosition)
            {
                soundPosition = useOwnerPosition ? position : specificPosition;
            }

            // Convert parameters to the tuple format expected by AudioManager
            (string name, float value)[] parameterTuples = null;
            if (parameters != null && parameters.Length > 0)
            {
                parameterTuples = new (string name, float value)[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterTuples[i] = (parameters[i].name, parameters[i].value);
                }
            }

            AudioManager.Instance.PlaySound(eventSO, soundPosition, parameterTuples);
        }
    }


    [System.Serializable]
    public struct AudioManagerParameter
    {
        public string name;
        public float  value;
    }
}
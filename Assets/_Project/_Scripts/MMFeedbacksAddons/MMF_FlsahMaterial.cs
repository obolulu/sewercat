using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

namespace MoreMountains.Feel
{
    /// <summary>
    /// A feedback that will make a material flash by modifying one of its properties (typically emission)
    /// </summary>
    [AddComponentMenu("")]
    [FeedbackPath("Renderer/Flash Effect")]
    public class MMF_FlashEffect : MMF_Feedback
    {
        [MMFInspectorGroup("Flash Settings", true, 37)]
        
        /// the material to affect when flashing
        [Tooltip("the material to affect when flashing")]
        public Material FlashMaterial;
        
        /// the property name to modify in the material
        [Tooltip("the property name to modify in the material")]
        public string PropertyName = "_EmissionColor";
        
        /// the color to apply during the flash
        [Tooltip("the color to apply during the flash")]
        [ColorUsage(true, true)]
        public Color FlashColor = Color.white;

        /// whether to use an ease curve for the flash intensity
        [Tooltip("whether to use an ease curve for the flash intensity")]
        public bool UseEaseCurve = true;
        
        /// the ease curve to use for the flash intensity
        [Tooltip("the ease curve to use for the flash intensity")]
        [MMFCondition("UseEaseCurve", true)]
        public AnimationCurve FlashIntensityCurve = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 1),
            new Keyframe(1, 0)
        );

        protected Color _initialColor;
        protected float _startTime;
        protected bool _playingFeedback;

        /// <summary>
        /// The duration of this feedback
        /// </summary>
        ///
        /*
        public override float FeedbackDuration
        {
            get { return ApplyTimeMultiplier(Timing.Duration); }
            set { Timing.Duration = value; }
        }
        */

        /// <summary>
        /// Custom initialization
        /// </summary>
        /// <param name="owner"></param>
        protected override void CustomInitialization(MMF_Player owner)
        {
            base.CustomInitialization(owner);
            _playingFeedback = false;
            if (FlashMaterial != null)
            {
                _initialColor = FlashMaterial.GetColor(PropertyName);
            }
        }

        /// <summary>
        /// On Play, starts flashing the material
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
        {
            if (!Active /*|| !FeedbackTypeAuthorized*/ || FlashMaterial == null)
            {
                return;
            }

            _playingFeedback = true;
            _startTime = FeedbackTime;
            Owner.StartCoroutine(FlashCoroutine(feedbacksIntensity));
        }

        /// <summary>
        /// Handles the flash coroutine
        /// </summary>
        protected virtual System.Collections.IEnumerator FlashCoroutine(float intensity)
        {
            float elapsed = 0f;
            float normalizedTime;
            
            while (elapsed < FeedbackDuration)
            {
                elapsed = FeedbackTime - _startTime;
                normalizedTime = elapsed / FeedbackDuration;
                
                float curveValue = UseEaseCurve ? FlashIntensityCurve.Evaluate(normalizedTime) : 1f - normalizedTime;
                float intensityMultiplier = ComputeIntensity(intensity, Owner.transform.position);
                
                Color currentColor = Color.Lerp(_initialColor, FlashColor * intensityMultiplier, curveValue);
                FlashMaterial.SetColor(PropertyName, currentColor);

                yield return null;
            }
            
            // Ensure we end with the initial color
            FlashMaterial.SetColor(PropertyName, _initialColor);
            _playingFeedback = false;
        }

        /// <summary>
        /// Stops the feedback
        /// </summary>
        /// <param name="position"></param>
        /// <param name="feedbacksIntensity"></param>
        protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active || !_playingFeedback || FlashMaterial == null)
            {
                return;
            }

            _playingFeedback = false;
            FlashMaterial.SetColor(PropertyName, _initialColor);
        }

        /// <summary>
        /// On restore, resets the initial color
        /// </summary>
        protected override void CustomRestoreInitialValues()
        {
            if (FlashMaterial != null)
            {
                FlashMaterial.SetColor(PropertyName, _initialColor);
            }
        }

        /// <summary>
        /// On disable, resets the initial color
        /// </summary>
        public override void OnDisable()
        {
            CustomRestoreInitialValues();
        }
    }
}
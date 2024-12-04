using UnityEngine;
/*
namespace _Project._Scripts.CameraEffects.Editor
{
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EffectChainData))]
public class CameraEffectChainEditor : Editor
{
    private SerializedProperty effectsProperty;
    private ReorderableList effectList;

    private void OnEnable()
    {
        effectsProperty = serializedObject.FindProperty("effects");

        effectList = new ReorderableList(serializedObject, effectsProperty, true, true, true, true)
        {
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Camera Effect Chain");
            },

            drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = effectsProperty.GetArrayElementAtIndex(index);
                SerializedProperty effect = element.FindPropertyRelative("effect");
                SerializedProperty delay = element.FindPropertyRelative("delay");
                SerializedProperty overrideSettings = element.FindPropertyRelative("overrideSettings");
                SerializedProperty customDuration = element.FindPropertyRelative("customDuration");
                SerializedProperty customEaseType = element.FindPropertyRelative("customEaseType");

                rect.y += 2;
                float lineHeight = EditorGUIUtility.singleLineHeight;

                // Effect
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, lineHeight), effect, new GUIContent("Effect"));

                // Delay
                rect.y += lineHeight + 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, lineHeight), delay, new GUIContent("Delay"));

                // Override Settings Toggle
                rect.y += lineHeight + 2;
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, lineHeight), overrideSettings, new GUIContent("Override Settings"));

                // Custom Duration and Ease Type (conditionally shown)
                if (overrideSettings.boolValue)
                {
                    rect.y += lineHeight + 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, lineHeight), customDuration, new GUIContent("Custom Duration"));

                    rect.y += lineHeight + 2;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, lineHeight), customEaseType, new GUIContent("Custom Ease Type"));
                }
            },

            elementHeightCallback = index =>
            {
                SerializedProperty element = effectsProperty.GetArrayElementAtIndex(index);
                SerializedProperty overrideSettings = element.FindPropertyRelative("overrideSettings");

                float lineHeight = EditorGUIUtility.singleLineHeight + 4;
                return overrideSettings.boolValue ? lineHeight * 5 : lineHeight * 3;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        effectList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
}
*/
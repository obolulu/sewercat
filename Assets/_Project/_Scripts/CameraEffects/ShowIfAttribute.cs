using UnityEditor;
using UnityEngine;

namespace _Project._Scripts.CameraEffects
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string conditionalField;
        public ShowIfAttribute(string conditionalField) => this.conditionalField = conditionalField;
    }
    #if UNITY_EDITOR
    // Custom Property Drawer
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var showIf      = attribute as ShowIfAttribute;
            var conditional = property.serializedObject.FindProperty(showIf.conditionalField);

            if (conditional != null && conditional.boolValue)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var showIf      = attribute as ShowIfAttribute;
            var conditional = property.serializedObject.FindProperty(showIf.conditionalField);

            if (conditional != null && conditional.boolValue)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            return 0;
        }
    }
    #endif
}
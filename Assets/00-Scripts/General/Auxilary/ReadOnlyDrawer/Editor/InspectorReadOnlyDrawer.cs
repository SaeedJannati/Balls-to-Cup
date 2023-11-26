using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BallsToCup.General.Editor
{
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
    public class InspectorReadOnlyDrawer : PropertyDrawer
    {
        #region Fields

        private string value;

        #endregion

        #region methods

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            value = property.propertyType switch
            {
                SerializedPropertyType.Boolean => property.boolValue.ToString(),
                SerializedPropertyType.Float=>property.floatValue.ToString("F3"),
                SerializedPropertyType.Integer=>property.intValue.ToString(),
                SerializedPropertyType.String=>property.stringValue,
                _ => "not supported!"
            };
            EditorGUI.LabelField(position,label.text,value);
        }

        #endregion
    }
}


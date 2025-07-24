using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CollisionSafety))]
//[CustomEditor(typeof(CollisionSafety))]
public class CollisionSafetyDrawer : PropertyDrawer
{
//    #region  SERIZLIZED_PORPERTIESD
//
//    private SerializedProperty _isPlacement;
//    private SerializedProperty _isGridPlacement;
//    private SerializedProperty _gridPlacementSettings;
//
//    #endregion SERIZLIZED_PORPERTIESD


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);
        
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            
            Rect currentRect = new Rect(position.x,
                                        position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                                        position.width,
                                        EditorGUIUtility.singleLineHeight);

            SerializedProperty isPlacement = property.FindPropertyRelative("_isPlacement");
            SerializedProperty isGridPlacement = property.FindPropertyRelative("_isGridPlacement");
            SerializedProperty gridPlacementSettings = property.FindPropertyRelative("_gridPlacementSettings");

            EditorGUI.PropertyField(currentRect, isPlacement);
            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            EditorGUI.PropertyField(currentRect, isGridPlacement);
            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (isGridPlacement.boolValue)
            {
                float gridPlacementHeight = EditorGUI.GetPropertyHeight(gridPlacementSettings, true);
                currentRect.height = gridPlacementHeight; 
                EditorGUI.PropertyField(currentRect, gridPlacementSettings, true);
            }

            EditorGUI.indentLevel--;
        }
     
        EditorGUI.EndProperty();
        
//        base.OnGUI(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (property.isExpanded)
        {
            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
            SerializedProperty isGridPlacement = property.FindPropertyRelative("_isGridPlacement");

            if (isGridPlacement != null && isGridPlacement.boolValue)
            {
                SerializedProperty gridPlacementSettings = property.FindPropertyRelative("_gridPlacementSettings");

                if (gridPlacementSettings != null)
                {
                    height += EditorGUI.GetPropertyHeight(gridPlacementSettings, true);
                }
            }

        }

        return height;
        
//        return base.GetPropertyHeight(property, label);
    }


//
//    private void OnEnable()
//    {
//        _isPlacement = serializedObject.FindProperty("_isPlacement");
//        _isGridPlacement = serializedObject.FindProperty("_isGridPlacement");
//        _gridPlacementSettings = serializedObject.FindProperty("_gridPlacementSettings");
//    }
//
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//
//        EditorGUILayout.PropertyField(_isPlacement);
//        EditorGUILayout.PropertyField(_isGridPlacement);
//
//        if (_isGridPlacement.boolValue)
//        {
//            EditorGUILayout.PropertyField(_gridPlacementSettings);
//        }
//        
//        serializedObject.ApplyModifiedProperties();
//    }
}

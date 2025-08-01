using UnityEditor;
using UnityEngine;

namespace Soor.Spawner2d.Editor
{
    [CustomPropertyDrawer(typeof(Limitation))]
    public class LimitationPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Start currentRect right after the foldout header
                Rect currentRect = new Rect(position.x,
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width,
                    EditorGUIUtility.singleLineHeight);

                // Find properties (do this within OnGUI as it's called per-frame for drawing)
                SerializedProperty limitationTypeProp = property.FindPropertyRelative("_limitationType");
                SerializedProperty limitTimeByProp = property.FindPropertyRelative("_limitTimeBy");
                SerializedProperty limitCountByProp = property.FindPropertyRelative("_limitCountBy");
                SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("onSpawnStartEvent");
                SerializedProperty onLimitationReachedEventProp = property.FindPropertyRelative("onLimitationReachedEvent");
                SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("onSpawnEndEvent");

                // Draw _limitationType
                EditorGUI.PropertyField(currentRect, limitationTypeProp);
                currentRect.y += EditorGUI.GetPropertyHeight(limitationTypeProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true for safety

                // Conditionally draw _limitTimeBy or _limitCountBy
                if (limitationTypeProp.intValue == (int) Limitation.LimitationType.Time)
                {
                    EditorGUI.PropertyField(currentRect, limitTimeByProp);
                    currentRect.y += EditorGUI.GetPropertyHeight(limitTimeByProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true
                }
                else // LimitationType.Count
                {
                    EditorGUI.PropertyField(currentRect, limitCountByProp);
                    currentRect.y += EditorGUI.GetPropertyHeight(limitCountByProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true
                }

                // Draw onSpawnStartEvent
                EditorGUI.PropertyField(currentRect, onSpawnStartEventProp, true); // Use true for UnityEvents
                currentRect.y += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;

                // Draw onLimitationReachedEvent
                EditorGUI.PropertyField(currentRect, onLimitationReachedEventProp, true); // Use true for UnityEvents
                currentRect.y += EditorGUI.GetPropertyHeight(onLimitationReachedEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                
                // Draw onSpawnEndEvent
                EditorGUI.PropertyField(currentRect, onSpawnEndEventProp, true); // Use true for UnityEvents
                currentRect.y += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        // This method now accurately calculates the height
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Start with the height of the foldout header itself
            float totalHeight = EditorGUIUtility.singleLineHeight;

            // Only add height for contents if the foldout is expanded
            if (property.isExpanded)
            {
                // Find properties for height calculation (do this within GetPropertyHeight as well)
                SerializedProperty limitationTypeProp = property.FindPropertyRelative("_limitationType");
                SerializedProperty limitTimeByProp = property.FindPropertyRelative("_limitTimeBy");
                SerializedProperty limitCountByProp = property.FindPropertyRelative("_limitCountBy");
                SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("onSpawnStartEvent");
                SerializedProperty onLimitationReachedEventProp = property.FindPropertyRelative("onLimitationReachedEvent");
                SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("onSpawnEndEvent");

                // Add height for _limitationType
                if (limitationTypeProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(limitationTypeProp, true) + EditorGUIUtility.standardVerticalSpacing;
                }

                if (limitationTypeProp != null) // Ensure limitationTypeProp is found before checking its value
                {
                    if (limitationTypeProp.intValue == (int) Limitation.LimitationType.Time)
                    {
                        if (limitTimeByProp != null)
                        {
                            totalHeight += EditorGUI.GetPropertyHeight(limitTimeByProp, true) + EditorGUIUtility.standardVerticalSpacing;
                        }
                    }
                    else // LimitationType.Count
                    {
                        if (limitCountByProp != null)
                        {
                            totalHeight += EditorGUI.GetPropertyHeight(limitCountByProp, true) + EditorGUIUtility.standardVerticalSpacing;
                        }
                    }
                }

                // Add height for onSpawnStartEvent
                if (onSpawnEndEventProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                }
                
                // Add height for onSpawnStartEvent
                if (onSpawnStartEventProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                }
                
                // Add height for onLimitationReachedEvent
                if (onLimitationReachedEventProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(onLimitationReachedEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return totalHeight;
        }
    }
}
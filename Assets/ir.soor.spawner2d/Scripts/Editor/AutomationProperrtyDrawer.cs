using UnityEditor;
using UnityEngine;

namespace Soor.Spawner2d.Editor
{
    [CustomPropertyDrawer(typeof(Automation))]
    public class AutomationPropertyDrawer : PropertyDrawer
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
                    EditorGUIUtility.singleLineHeight); // Initial height for the first field

                // Find properties. Doing this inside OnGUI is fine for PropertyDrawers.
                SerializedProperty perSpawnIntervalProp = property.FindPropertyRelative("_perSpawnInterval");
                SerializedProperty stopSpawningAutomaticallyProp =
                    property.FindPropertyRelative("_stopSpawningAutomatically");
                SerializedProperty limitationSettingsProp = property.FindPropertyRelative("_limitationSettings");
                SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("onSpawnStartEvent");
                SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("onSpawnEndEvent");

                // Draw _perSpawnInterval
                EditorGUI.PropertyField(currentRect, perSpawnIntervalProp);
                currentRect.y += EditorGUI.GetPropertyHeight(perSpawnIntervalProp, true) +
                                 EditorGUIUtility.standardVerticalSpacing;

                // Draw _stopSpawningAutomatically
                EditorGUI.PropertyField(currentRect, stopSpawningAutomaticallyProp);
                currentRect.y += EditorGUI.GetPropertyHeight(stopSpawningAutomaticallyProp, true) +
                                 EditorGUIUtility.standardVerticalSpacing;

                // Conditionally draw _limitationSettings
                if (stopSpawningAutomaticallyProp.boolValue)
                {
                    currentRect.y += EditorGUIUtility.singleLineHeight / 2;
                    EditorGUI.PropertyField(currentRect, limitationSettingsProp, true);
                    currentRect.y += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) +
                                     EditorGUIUtility.standardVerticalSpacing;
                    currentRect.y += EditorGUIUtility.singleLineHeight;
                }

                if (!stopSpawningAutomaticallyProp.boolValue)
                {
                    // Draw onSpawnStartEvent
                    EditorGUI.PropertyField(currentRect, onSpawnStartEventProp, true); // Use 'true' for UnityEvent
                    currentRect.y += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) +
                                     EditorGUIUtility.standardVerticalSpacing;

                    // Draw onSpawnEndEvent
                    EditorGUI.PropertyField(currentRect, onSpawnEndEventProp, true); // Use 'true' for UnityEvent
                    currentRect.y += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) +
                                     EditorGUIUtility.standardVerticalSpacing;
                }
                
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Start with the height of the foldout header itself
            float totalHeight = EditorGUIUtility.singleLineHeight;

            // If the foldout is expanded, add the height of its contents
            if (property.isExpanded)
            {
                SerializedProperty stopSpawningAutomaticallyProp = property.FindPropertyRelative("_stopSpawningAutomatically");
                SerializedProperty limitationSettingsProp = property.FindPropertyRelative("_limitationSettings");

                if (stopSpawningAutomaticallyProp != null &&!stopSpawningAutomaticallyProp.boolValue)
                {
                    SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("onSpawnStartEvent");
                    
                    // Add height for onSpawnStartEvent
                    if (onSpawnStartEventProp != null)
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                    }
                    
                    SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("onSpawnEndEvent");
                    
                    // Add height for onSpawnEndEvent
                    if (onSpawnEndEventProp != null)
                    {
                        totalHeight += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                // Conditionally add height for _limitationSettings
                if (stopSpawningAutomaticallyProp != null && stopSpawningAutomaticallyProp.boolValue)
                {
                    if (limitationSettingsProp != null)
                    {
                        totalHeight += EditorGUIUtility.singleLineHeight;
                        totalHeight += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) + EditorGUIUtility.standardVerticalSpacing;
                        totalHeight += EditorGUIUtility.singleLineHeight;
                    }
                }
            }

            return totalHeight;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace Soor.Spawner2d.Editor
{
    [CustomPropertyDrawer(typeof(Automation))]
    public class AutomationPropertyDrawer : PropertyDrawer // Corrected typo in class name: PropertyDrawer
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
                SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
                SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("_onSpawnEndEvent");

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
                    currentRect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(currentRect, limitationSettingsProp,
                        true); // Use 'true' for complex properties/nested drawers
                    currentRect.y += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) +
                                     EditorGUIUtility.standardVerticalSpacing;
                    currentRect.y += EditorGUIUtility.singleLineHeight;
                }

                // Draw _onSpawnStartEvent
                EditorGUI.PropertyField(currentRect, onSpawnStartEventProp, true); // Use 'true' for UnityEvent
                currentRect.y += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) +
                                 EditorGUIUtility.standardVerticalSpacing;

                // Draw _onSpawnEndEvent
                EditorGUI.PropertyField(currentRect, onSpawnEndEventProp, true); // Use 'true' for UnityEvent
                currentRect.y += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) +
                                 EditorGUIUtility.standardVerticalSpacing;

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
                // Find properties for height calculation.
                // These should match the ones drawn in OnGUI.
                SerializedProperty perSpawnIntervalProp = property.FindPropertyRelative("_perSpawnInterval");
                SerializedProperty stopSpawningAutomaticallyProp =
                    property.FindPropertyRelative("_stopSpawningAutomatically");
                SerializedProperty limitationSettingsProp = property.FindPropertyRelative("_limitationSettings");
                SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
                SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("_onSpawnEndEvent");

                // Add height for each property that is consistently drawn
                if (perSpawnIntervalProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(perSpawnIntervalProp, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                }

                if (stopSpawningAutomaticallyProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(stopSpawningAutomaticallyProp, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                }

                // Conditionally add height for _limitationSettings
                if (stopSpawningAutomaticallyProp != null && stopSpawningAutomaticallyProp.boolValue)
                {
                    if (limitationSettingsProp != null)
                    {
                        totalHeight += EditorGUIUtility.singleLineHeight;

                        totalHeight += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) +
                                       EditorGUIUtility.standardVerticalSpacing;

                        totalHeight += EditorGUIUtility.singleLineHeight;
                    }
                }

                // Add height for _onSpawnStartEvent
                if (onSpawnStartEventProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                }

                // Add height for _onSpawnEndEvent
                if (onSpawnEndEventProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) +
                                   EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return totalHeight;
        }
    }
}
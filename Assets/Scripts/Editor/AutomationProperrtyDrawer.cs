//using UnityEditor;
//using UnityEngine;
//
//[CustomPropertyDrawer(typeof(Automation))]
//public class AutomationProperrtyDrawer: PropertyDrawer
//{
////    
////    private SerializedProperty _perSpawnInterval;
////    private SerializedProperty _stopSpawningAutomatically;
////    private SerializedProperty _limitationSettings;
////    private SerializedProperty _onSpawnStartEvent;
////    private SerializedProperty _onSpawnEndEvent;
////    
//    
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);
//        
//        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
//
//        if (property.isExpanded)
//        {
//            EditorGUI.indentLevel++;
//            
//            Rect currentRect = new Rect(position.x,
//                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
//                position.width,
//                EditorGUIUtility.singleLineHeight);
//
//            SerializedProperty _perSpawnInterval = property.FindPropertyRelative("_perSpawnInterval");
//            SerializedProperty _stopSpawningAutomatically = property.FindPropertyRelative("_stopSpawningAutomatically");
//            SerializedProperty _limitationSettings = property.FindPropertyRelative("_limitationSettings");
//            SerializedProperty _onSpawnStartEvent = property.FindPropertyRelative("_onSpawnStartEvent");
//            SerializedProperty _onSpawnEndEvent = property.FindPropertyRelative("_onSpawnEndEvent");
//
//
//            
//            EditorGUI.PropertyField(currentRect, _perSpawnInterval);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//
//            EditorGUI.PropertyField(currentRect, _stopSpawningAutomatically);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//
//
//            if (_stopSpawningAutomatically.boolValue)
//            {
//                EditorGUI.PropertyField(currentRect, _limitationSettings);
//                currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            }
//            
//            EditorGUI.PropertyField(currentRect, _onSpawnStartEvent);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//
//            EditorGUI.PropertyField(currentRect, _onSpawnEndEvent);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//
//            EditorGUI.indentLevel--;
//        }
//        
//        EditorGUI.EndProperty();
//    }
//
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        float height = EditorGUIUtility.singleLineHeight;
//
//        if (property.isExpanded)
//        {
//            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
//            SerializedProperty _stopSpawningAutomatically = property.FindPropertyRelative("_stopSpawningAutomatically");
//
//            if (_stopSpawningAutomatically != null && _stopSpawningAutomatically.boolValue)
//            {
//                SerializedProperty _limitationSettings = property.FindPropertyRelative("_limitationSettings");
//                
//                if (_limitationSettings != null)
//                {
//                    height += EditorGUI.GetPropertyHeight(_limitationSettings, true);
//                }
//            }
//            
////            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
////            
////            SerializedProperty _onSpawnStartEvent = property.FindPropertyRelative("_onSpawnStartEvent");
////            
////            if (_onSpawnStartEvent != null)
////            {
////                height += EditorGUI.GetPropertyHeight(_onSpawnStartEvent, true);
////            }
////            
////            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
////            
////            SerializedProperty _onSpawnEndEvent = property.FindPropertyRelative("_onSpawnEndEvent");
////            
////            if (_onSpawnStartEvent != null)
////            {
////                height += EditorGUI.GetPropertyHeight(_onSpawnEndEvent, true);
////            }
//
//            
//        }
//
//        return height;
//    }
//    
//}














using UnityEditor;
using UnityEngine;
using UnityEngine.Events; // Make sure this is included if your events are UnityEvent

// Ensure your Automation class is defined like this (for context):
// [System.Serializable]
// public class Automation
// {
//     [SerializeField] private float _perSpawnInterval = 1f;
//     [SerializeField] private bool _stopSpawningAutomatically = false;
//     [SerializeField] private Limitation _limitationSettings = new Limitation(); // Assuming Limitation is also [Serializable]
//     [SerializeField] private UnityEvent _onSpawnStartEvent;
//     [SerializeField] private UnityEvent _onSpawnEndEvent;
// }


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
            SerializedProperty stopSpawningAutomaticallyProp = property.FindPropertyRelative("_stopSpawningAutomatically");
            SerializedProperty limitationSettingsProp = property.FindPropertyRelative("_limitationSettings");
            SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
            SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("_onSpawnEndEvent");

            // Draw _perSpawnInterval
            EditorGUI.PropertyField(currentRect, perSpawnIntervalProp);
            currentRect.y += EditorGUI.GetPropertyHeight(perSpawnIntervalProp, true) + EditorGUIUtility.standardVerticalSpacing;

            // Draw _stopSpawningAutomatically
            EditorGUI.PropertyField(currentRect, stopSpawningAutomaticallyProp);
            currentRect.y += EditorGUI.GetPropertyHeight(stopSpawningAutomaticallyProp, true) + EditorGUIUtility.standardVerticalSpacing;

            // Conditionally draw _limitationSettings
            if (stopSpawningAutomaticallyProp.boolValue)
            {
                EditorGUI.PropertyField(currentRect, limitationSettingsProp, true); // Use 'true' for complex properties/nested drawers
                currentRect.y += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Draw _onSpawnStartEvent
            EditorGUI.PropertyField(currentRect, onSpawnStartEventProp, true); // Use 'true' for UnityEvent
            currentRect.y += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;

            // Draw _onSpawnEndEvent
            EditorGUI.PropertyField(currentRect, onSpawnEndEventProp, true); // Use 'true' for UnityEvent
            currentRect.y += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
            
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
            SerializedProperty stopSpawningAutomaticallyProp = property.FindPropertyRelative("_stopSpawningAutomatically");
            SerializedProperty limitationSettingsProp = property.FindPropertyRelative("_limitationSettings");
            SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
            SerializedProperty onLimitationReachedEventProp = property.FindPropertyRelative("_onLimitationReachedEvent");
            
            
            SerializedProperty onSpawnEndEventProp = property.FindPropertyRelative("_onSpawnEndEvent");
            

            // Add height for each property that is consistently drawn
            if (perSpawnIntervalProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(perSpawnIntervalProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            if (stopSpawningAutomaticallyProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(stopSpawningAutomaticallyProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Conditionally add height for _limitationSettings
            if (stopSpawningAutomaticallyProp != null && stopSpawningAutomaticallyProp.boolValue)
            {
                if (limitationSettingsProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(limitationSettingsProp, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            
            // Add height for _onSpawnStartEvent
            if (onSpawnStartEventProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Add height for _onSpawnEndEvent
            if (onSpawnEndEventProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(onSpawnEndEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Optional: You might want to remove the very last standardVerticalSpacing
            // if you don't want extra space at the bottom of your foldout content.
            // Example: if (totalHeight > EditorGUIUtility.singleLineHeight) totalHeight -= EditorGUIUtility.standardVerticalSpacing;
        }

        return totalHeight;
    }
}
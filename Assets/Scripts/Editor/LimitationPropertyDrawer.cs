//using UnityEditor;
//using UnityEngine;
//
//[CustomPropertyDrawer(typeof(Limitation))]
//public class LimitationPropertyDrawer : PropertyDrawer
//{
//    private SerializedProperty _limitationType;
//    private SerializedProperty _limitTimeBy;
//    private SerializedProperty _limitCountBy;
//    private SerializedProperty _onSpawnStartEvent;
//    private SerializedProperty _onLimitationReachedEvent;
//
//    
//
//
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//
////        SetSerializedProperties(property);
//        
//        EditorGUI.BeginProperty(position, label, property);
//        
//        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
//
//
//        if (property.isExpanded)
//        {
//            
//            EditorGUI.indentLevel++;
//            
//            Rect currentRect = new Rect(position.x,
//                position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
//                position.width,
//                EditorGUIUtility.singleLineHeight);
//
//            
//            _limitationType = property.FindPropertyRelative("_limitationType");
//
//            EditorGUI.PropertyField(currentRect, _limitationType);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//
//            if (_limitationType.intValue == (int)Limitation.LimitationType.Time)
//            {
//                _limitTimeBy = property.FindPropertyRelative("_limitTimeBy");
//
//                
//                EditorGUI.PropertyField(currentRect, _limitTimeBy);
//                currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            }
//            else
//            {
//                _limitCountBy = property.FindPropertyRelative("_limitCountBy");
//                
//                EditorGUI.PropertyField(currentRect, _limitCountBy);
//                currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            }
//            
//            _onSpawnStartEvent = property.FindPropertyRelative("_onSpawnStartEvent");
//
//            EditorGUI.PropertyField(currentRect, _onSpawnStartEvent);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            
//            _onLimitationReachedEvent = property.FindPropertyRelative("_onLimitationReachedEvent");
//
//            EditorGUI.PropertyField(currentRect, _onLimitationReachedEvent);
//            currentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
//            
//            EditorGUI.indentLevel--;
//        }
//        
//        EditorGUI.EndProperty();
//    }
//
////    private void SetSerializedProperties(SerializedProperty property)
////    {
//////        if (_limitationType == null)
//////        {
//////            _limitationType = property.FindPropertyRelative("_limitationType");
//////        }
////
//////        if (_limitTimeBy == null)
//////        {
//////            _limitTimeBy = property.FindPropertyRelative("_limitTimeBy");
//////        }
////        
//////        if (_limitCountBy == null)
//////        {
//////            _limitCountBy = property.FindPropertyRelative("_limitCountBy");
//////        }
////
//////        if (_onSpawnStartEvent == null)
//////        {
//////            _onSpawnStartEvent = property.FindPropertyRelative("_onSpawnStartEvent");
//////        }
////        
//////        if (_onLimitationReachedEvent == null)
//////        {
//////            _onLimitationReachedEvent = property.FindPropertyRelative("_onLimitationReachedEvent");
//////        }
////    }
//
//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        float height = EditorGUIUtility.singleLineHeight;
//
//        if (property.isExpanded)
//        {
//            height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
//
//            _limitationType = property.FindPropertyRelative("_limitationType");
//            
//            if (_limitationType != null && _limitationType.intValue == (int)Limitation.LimitationType.Time)
//            {
//                _limitTimeBy = property.FindPropertyRelative("_limitTimeBy");
//
//                if (_limitTimeBy != null)
//                {
//                    height += EditorGUI.GetPropertyHeight(_limitTimeBy, false);
//
//                }
//                
//            }
//            else
//            {
//                _limitCountBy = property.FindPropertyRelative("_limitCountBy");
//
//                if (_limitCountBy != null)
//                {
//                    height += EditorGUI.GetPropertyHeight(_limitCountBy, false);
//
//                }
//                
//            }
//            
//        }
//
//        return height;
//    }
//}



using UnityEditor;
using UnityEngine;
using UnityEngine.Events; // Make sure this is included if your events are UnityEvent

// Make sure your Limitation class looks something like this (for context):
// [System.Serializable]
// public class Limitation
// {
//     public enum LimitationType { Time, Count }
//
//     [SerializeField] private LimitationType _limitationType;
//     [SerializeField] private float _limitTimeBy; // Used if type is Time
//     [SerializeField] private int _limitCountBy;  // Used if type is Count
//     [SerializeField] private UnityEvent _onSpawnStartEvent;
//     [SerializeField] private UnityEvent _onLimitationReachedEvent;
// }


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
            SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
            SerializedProperty onLimitationReachedEventProp = property.FindPropertyRelative("_onLimitationReachedEvent");

            // Draw _limitationType
            EditorGUI.PropertyField(currentRect, limitationTypeProp);
            currentRect.y += EditorGUI.GetPropertyHeight(limitationTypeProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true for safety

            // Conditionally draw _limitTimeBy or _limitCountBy
            if (limitationTypeProp.intValue == (int)Limitation.LimitationType.Time)
            {
                EditorGUI.PropertyField(currentRect, limitTimeByProp);
                currentRect.y += EditorGUI.GetPropertyHeight(limitTimeByProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true
            }
            else // LimitationType.Count
            {
                EditorGUI.PropertyField(currentRect, limitCountByProp);
                currentRect.y += EditorGUI.GetPropertyHeight(limitCountByProp, true) + EditorGUIUtility.standardVerticalSpacing; // Use true
            }

            // Draw _onSpawnStartEvent
            EditorGUI.PropertyField(currentRect, onSpawnStartEventProp, true); // Use true for UnityEvents
            currentRect.y += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;

            // Draw _onLimitationReachedEvent
            EditorGUI.PropertyField(currentRect, onLimitationReachedEventProp, true); // Use true for UnityEvents
            currentRect.y += EditorGUI.GetPropertyHeight(onLimitationReachedEventProp, true) + EditorGUIUtility.standardVerticalSpacing;


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
            SerializedProperty onSpawnStartEventProp = property.FindPropertyRelative("_onSpawnStartEvent");
            SerializedProperty onLimitationReachedEventProp = property.FindPropertyRelative("_onLimitationReachedEvent");

            // Add height for _limitationType
            if (limitationTypeProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(limitationTypeProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Conditionally add height for _limitTimeBy or _limitCountBy
            if (limitationTypeProp != null) // Ensure limitationTypeProp is found before checking its value
            {
                if (limitationTypeProp.intValue == (int)Limitation.LimitationType.Time)
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

            // Add height for _onSpawnStartEvent
            if (onSpawnStartEventProp != null)
            {
                totalHeight += EditorGUI.GetPropertyHeight(onSpawnStartEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Add height for _onLimitationReachedEvent
            if (onLimitationReachedEventProp != null)
            {
                   totalHeight += EditorGUI.GetPropertyHeight(onLimitationReachedEventProp, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            
            // Optional: Remove the last standardVerticalSpacing as it's not needed after the very last element
            // This can make the bottom spacing look a bit cleaner.
            // Be careful with this if your last element is sometimes conditional.
            // For now, let's keep it simple without removing the last one.
        }

        return totalHeight;
    }
}
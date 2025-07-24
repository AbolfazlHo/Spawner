using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : Editor
{
    #region  SERIZLIZED_PORPERTIESD

    private SerializedProperty _spawnAreaGameObject;
    private SerializedProperty _spawnables;
    private SerializedProperty _spawnAutomaticaly;
    private SerializedProperty _spawnAutomationSettings;
    private SerializedProperty _isCollisionSafe;
    private SerializedProperty _collisionSafetySettings;
    private SerializedProperty _onSpawnableSpawnedEvent;

    #endregion SERIZLIZED_PORPERTIESD

//    private Editor _collisionSafetyEditor;
    
    private void OnEnable()
    {
        _spawnAreaGameObject = serializedObject.FindProperty("_spawnAreaGameObject");
        _spawnables = serializedObject.FindProperty("_spawnables");
        _spawnAutomaticaly = serializedObject.FindProperty("_spawnAutomaticaly");
        _spawnAutomationSettings = serializedObject.FindProperty("_spawnAutomationSettings");
        _isCollisionSafe = serializedObject.FindProperty("_isCollisionSafe");
        
        
        _collisionSafetySettings = serializedObject.FindProperty("_collisionSafetySettings");
        
        
        
        _onSpawnableSpawnedEvent = serializedObject.FindProperty("_onSpawnableSpawnedEvent");



//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings.Copy(), typeof(CollisionSafetyPropertyDrawer));
//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings.objectReferenceValue, typeof(CollisionSafetyPropertyDrawer));
//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings, typeof(CollisionSafety));
//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings.exposedReferenceValue, typeof(CollisionSafetyPropertyDrawer));
//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings.objectReferenceValue, typeof(CollisionSafetyPropertyDrawer));
//        _collisionSafetyEditor = Editor.CreateEditor(_collisionSafetySettings.objectReferenceValue, typeof(CollisionSafetyPropertyDrawer));
//        _collisionSafetyEditor = Editor.CreateInstance<CollisionSafetyPropertyDrawer>();
//        _collisionSafetyEditor = Editor.


    }

//    private void OnDisable()
//    {
//        if (_collisionSafetyEditor != null)
//        {
//            DestroyImmediate(_collisionSafetyEditor);
//        }
//    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Spawn settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(_spawnAreaGameObject);
        EditorGUILayout.PropertyField(_spawnables);
        
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_spawnAutomaticaly);
        
        if (_spawnAutomaticaly.boolValue)
        {
            EditorGUILayout.PropertyField(_spawnAutomationSettings);
        }
  
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_isCollisionSafe);

        if (_isCollisionSafe.boolValue)
        {
            EditorGUILayout.PropertyField(_collisionSafetySettings);

//
//            if (_collisionSafetyEditor != null)
//            {
//                _collisionSafetyEditor.OnInspectorGUI();
//            }
//            else
//            {
//                EditorGUILayout.HelpBox("CollisionSafety editor could not be created", MessageType.Error);
//            }
            
            
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Events");
        EditorGUILayout.PropertyField(_onSpawnableSpawnedEvent);

        serializedObject.ApplyModifiedProperties();
    }
}

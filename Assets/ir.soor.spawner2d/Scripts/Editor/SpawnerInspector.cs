using UnityEditor;

namespace Soor.Spawner2d.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerInspector : UnityEditor.Editor
    {
        #region  SERIZLIZED_PORPERTIESD

        private SerializedProperty _spawnAreaGameObject;
        private SerializedProperty _spawnables;
        private SerializedProperty _spawnAutomaticaly;
        private SerializedProperty _spawnAutomationSettings;
        private SerializedProperty _isCollisionSafe;
        private SerializedProperty _collisionSafetySettings;
        private SerializedProperty _onSpawnableSpawnedEvent;
        private SerializedProperty _spawnableTag;

        #endregion SERIZLIZED_PORPERTIESD

        private void OnEnable()
        {
            _spawnAreaGameObject = serializedObject.FindProperty("_spawnAreaGameObject");
            _spawnables = serializedObject.FindProperty("_spawnables");
            _spawnAutomaticaly = serializedObject.FindProperty("_spawnAutomaticaly");
            _spawnAutomationSettings = serializedObject.FindProperty("_spawnAutomationSettings");
            _isCollisionSafe = serializedObject.FindProperty("_isCollisionSafe");
            _collisionSafetySettings = serializedObject.FindProperty("_collisionSafetySettings");
//            _onSpawnableSpawnedEvent = serializedObject.FindProperty("_onSpawnableSpawnedEvent");
            _spawnableTag = serializedObject.FindProperty("_spawnableTag");
        }

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
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_spawnableTag);

            EditorGUILayout.Space();
//            EditorGUILayout.LabelField("Events");
//            EditorGUILayout.PropertyField(_onSpawnableSpawnedEvent);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
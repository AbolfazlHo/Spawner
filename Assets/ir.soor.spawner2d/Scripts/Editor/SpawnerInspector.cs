using UnityEditor;

namespace Soor.Spawner2d.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerInspector : UnityEditor.Editor
    {
        #region  SERIZLIZED_PORPERTIESD

        private SerializedProperty _spawnAreaGameObject;
        private SerializedProperty _spawnables;
        private SerializedProperty _spawnAutomatically;
        private SerializedProperty _spawnAutomationSettings;
        private SerializedProperty _isCollisionSafe;
        private SerializedProperty _collisionSafetySettings;
        private SerializedProperty _spawnableTag;
        private SerializedProperty _useObjectPool;
        private SerializedProperty _poolerSettings;

        #endregion SERIZLIZED_PORPERTIESD

        private void OnEnable()
        {
            _spawnAreaGameObject = serializedObject.FindProperty("_spawnAreaGameObject");
            _spawnables = serializedObject.FindProperty("_spawnables");
            _spawnAutomatically = serializedObject.FindProperty("_spawnAutomatically");
            _spawnAutomationSettings = serializedObject.FindProperty("_spawnAutomationSettings");
            _isCollisionSafe = serializedObject.FindProperty("_isCollisionSafe");
            _collisionSafetySettings = serializedObject.FindProperty("_collisionSafetySettings");
            _spawnableTag = serializedObject.FindProperty("_spawnableTag");
            _useObjectPool = serializedObject.FindProperty("_useObjectPool");
            _poolerSettings = serializedObject.FindProperty("_poolerSettings");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Spawn settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spawnAreaGameObject);
            EditorGUILayout.PropertyField(_spawnables);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_spawnAutomatically);

            if (_spawnAutomatically.boolValue)
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
            EditorGUILayout.PropertyField(_useObjectPool);

            if (_useObjectPool.boolValue)
            {
                EditorGUILayout.PropertyField(_poolerSettings);
            }
            
            EditorGUILayout.Space();


            serializedObject.ApplyModifiedProperties();
        }
    }
}
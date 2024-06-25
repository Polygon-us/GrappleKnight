using Enemies.BasicEnemy.Weapons.Bases;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MeleeWeapon))]
    public class MeleeWeaponEditor : UnityEditor.Editor
    {
        private SerializedProperty 
            _pointA_prop,
            _playerLayer_prop,
            _radius_prop,
            _size_prop,
            _angle_prop,
            _pointB_prop;

        void OnEnable () {
        
            _pointA_prop = serializedObject.FindProperty ("pointA");
            _playerLayer_prop = serializedObject.FindProperty ("playerLayer");
            
            _radius_prop = serializedObject.FindProperty ("radius");
            
            _size_prop = serializedObject.FindProperty ("size");
            _angle_prop = serializedObject.FindProperty ("angle");
            
            _pointB_prop = serializedObject.FindProperty ("pointB");
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "pointA", "playerLayer", "radius", "size", "angle", "pointB");

            MeleeWeapon meleeWeapon = (MeleeWeapon)target;
            
            switch (meleeWeapon.areaType)
            {
                case AreaType.Area:
                    EditorGUILayout.PropertyField(_pointA_prop, new GUIContent("Initial Point"));
                    EditorGUILayout.PropertyField(_pointB_prop, new GUIContent("Final Point"));
                    EditorGUILayout.PropertyField(_playerLayer_prop, new GUIContent("Player LayerMask"));
                    break;
                case AreaType.Box:
                    EditorGUILayout.PropertyField(_pointA_prop, new GUIContent("Center Point"));
                    EditorGUILayout.PropertyField(_size_prop, new GUIContent("Size"));
                    EditorGUILayout.PropertyField(_angle_prop, new GUIContent("Angle"));
                    EditorGUILayout.PropertyField(_playerLayer_prop, new GUIContent("Player LayerMask"));
                    break;
                case AreaType.Circle:
                    EditorGUILayout.PropertyField(_pointA_prop, new GUIContent("Center Point"));
                    EditorGUILayout.PropertyField(_radius_prop, new GUIContent("Radius"));
                    EditorGUILayout.PropertyField(_playerLayer_prop, new GUIContent("Player LayerMask"));
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

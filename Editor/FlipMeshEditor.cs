using UnityEngine;
using UnityEditor;
using nadena.dev.ndmf.preview;

namespace com.aoyon.flip_mesh
{
    [CustomEditor(typeof(FlipMesh))]
    public class FlipMeshEditor: Editor
    {
        private FlipMesh _target;
        private void OnEnable()
        {
            _target = target as FlipMesh;
            var skinnedMeshRenderer = _target.GetComponent<SkinnedMeshRenderer>();
        }

        private void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true);
            while(iterator.NextVisible(false))
            {
                EditorGUILayout.PropertyField(iterator);
            }
            serializedObject.ApplyModifiedProperties();
            RenderPreviewToggle(FlipMeshPreview.ToggleNode);
        }

        public static void RenderPreviewToggle(TogglablePreviewNode toggleNode)
        {
            if (GUILayout.Button(toggleNode.IsEnabled.Value ? "Stop Preview" : "Preview"))
            {
                toggleNode.IsEnabled.Value = !toggleNode.IsEnabled.Value;
            }
        }
    }
}
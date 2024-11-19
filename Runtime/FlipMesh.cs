using UnityEngine;
using VRC.SDKBase;

namespace com.aoyon.flip_mesh
{
    [DisallowMultipleComponent]
    [AddComponentMenu("FlipMesh")]
    [RequireComponent(typeof(Renderer))]
    public class FlipMesh: MonoBehaviour, IEditorOnly
    {
        public bool FlipX = false;
        public bool FlipY = false;
        public bool FlipZ = false;
    }
}
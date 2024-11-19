using UnityEngine;

namespace com.aoyon.flip_mesh
{
    public class FlipMeshProcessor
    {
        public static Mesh FlipMesh(Mesh mesh, bool flipX = false, bool flipY = false, bool flipZ = false)
        {
            var newMesh = Object.Instantiate(mesh);

            var vertices = newMesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (flipX) vertices[i].x = -vertices[i].x;
                if (flipY) vertices[i].y = -vertices[i].y;
                if (flipZ) vertices[i].z = -vertices[i].z;
            }
            newMesh.vertices = vertices;
            newMesh.RecalculateNormals();
            newMesh.RecalculateBounds();

            return newMesh;
        }

        public static void AssignMesh(Renderer renderer, Mesh mesh)
        {
            switch (renderer)
            {
                case MeshRenderer meshrenderer:
                    var meshfilter = meshrenderer.GetComponent<MeshFilter>();
                    if (meshfilter == null) return;
                    meshfilter.sharedMesh = mesh;
                    break;
                case SkinnedMeshRenderer skinnedMeshRenderer:
                    skinnedMeshRenderer.sharedMesh = mesh;
                    break;
            }
        }

        public static Mesh GetMesh(Renderer renderer)
        {
            switch (renderer)
            {
                case MeshRenderer meshrenderer:
                    var meshfilter = meshrenderer.GetComponent<MeshFilter>();
                    return meshfilter?.sharedMesh;
                case SkinnedMeshRenderer skinnedMeshRenderer:
                    return skinnedMeshRenderer.sharedMesh;
                default:
                    return null;
            }
        }

    }
}
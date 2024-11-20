using System.Collections.Generic;
using UnityEngine;
using nadena.dev.modular_avatar.core;

namespace com.aoyon.flip_mesh
{
    public class FlipMeshProcessor
    {
        public static void Apply(GameObject root)
        {
            foreach (var component in root.GetComponentsInChildren<FlipMesh>(true))
            {
                var renderer = component.GetComponent<Renderer>();
                
                if (!(renderer is SkinnedMeshRenderer or MeshRenderer)) {
                    Object.DestroyImmediate(component);
                    continue;
                };
                
                var mesh = GetMesh(renderer);
                var modifiedMesh = FlipPolygon(mesh);
                AssignMesh(renderer, modifiedMesh);

                if (renderer is SkinnedMeshRenderer smr)
                {
                    var bones = smr.bones;
                    for (int i = 0; i < bones.Length; i++)
                    {   
                        var origin = bones[i];
                        if (origin == null) continue;

                        var proxy = CreateProxy(origin, component);

                        bones[i] = proxy;

                    }
                    smr.bones = bones;
                }
                else if (renderer is MeshRenderer mr)
                {
                    var transform = mr.transform;
                    var scale = transform.localScale;
                    if (component.FlipX) scale.x = -scale.x;
                    if (component.FlipY) scale.y = -scale.y;
                    if (component.FlipZ) scale.z = -scale.z;
                    transform.localScale = scale;
                }

                Object.DestroyImmediate(component);
            }

            Transform CreateProxy(Transform origin, FlipMesh component)
            {
                var proxyObject = new GameObject("FlipProxy");
                var proxyTransform = proxyObject.transform;
                proxyObject.AddComponent<ModularAvatarPBBlocker>();

                proxyTransform.SetParent(origin, false);
                proxyTransform.localPosition = Vector3.zero;
                proxyTransform.localRotation = Quaternion.identity;

                var scale = Vector3.one;
                if (component.FlipX) scale.x = -1;
                if (component.FlipY) scale.y = -1;
                if (component.FlipZ) scale.z = -1;
                proxyTransform.localScale = scale;

                return proxyTransform;
            }
        }

        private static Mesh FlipPolygon(Mesh mesh)
        {
            var newMesh = Object.Instantiate(mesh);

            var subMeshCount = newMesh.subMeshCount;
            for (int subMeshIndex = 0; subMeshIndex < subMeshCount; subMeshIndex++)
            {
                var triangles = newMesh.GetTriangles(subMeshIndex);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    var temp = triangles[i];
                    triangles[i] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                newMesh.SetTriangles(triangles, subMeshIndex);
            }
            return newMesh;
        }

        // Preview用
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
using UnityEngine;
using nadena.dev.ndmf;

namespace com.aoyon.flip_mesh
{
    public class FlipMeshPass : Pass<FlipMeshPass>
    {
        protected override void Execute(BuildContext context)
        {
            var components = context.AvatarRootObject.GetComponentsInChildren<FlipMesh>(true);

            foreach (var component in components)
            {
                var renderer = component.GetComponent<Renderer>();
                var mesh = FlipMeshProcessor.GetMesh(renderer);

                var modifiedMesh = FlipMeshProcessor.FlipMesh(mesh, component.FlipX, component.FlipY, component.FlipZ);
                FlipMeshProcessor.AssignMesh(renderer, modifiedMesh);

                Object.DestroyImmediate(component);
            }
        }
    }
}
            

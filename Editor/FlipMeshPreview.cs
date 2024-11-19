using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using nadena.dev.ndmf.preview;

namespace com.aoyon.flip_mesh
{
    internal class FlipMeshPreview : IRenderFilter
    {
        public static TogglablePreviewNode ToggleNode = TogglablePreviewNode.Create(
            () => "FlipMesh",
            qualifiedName: "com.aoyon.flip_mesh/FlipMeshPreview",
            true
        );
        
        public IEnumerable<TogglablePreviewNode> GetPreviewControlNodes()
        {
            yield return ToggleNode;
        }

        public bool IsEnabled(ComputeContext context)
        {
            return context.Observe(ToggleNode.IsEnabled);
        }

        public ImmutableList<RenderGroup> GetTargetGroups(ComputeContext context)
        {
            return context.GetComponentsByType<FlipMesh>()
                .Select(component => (component, context.GetComponent<Renderer>(component.gameObject)))
                .Select(pair => RenderGroup.For(pair.Item2).WithData(new FlipMesh[] { pair.Item1 }))
                .ToImmutableList();
        }

        public Task<IRenderFilterNode> Instantiate(RenderGroup group, IEnumerable<(Renderer, Renderer)> proxyPairs, ComputeContext context)
        {
            var component = group.GetData<FlipMesh[]>().First();
            context.Observe(component, c => c.FlipX);
            context.Observe(component, c => c.FlipY);
            context.Observe(component, c => c.FlipZ);

            var pair = proxyPairs.First();
            var proxy = pair.Item2;

            var mesh = FlipMeshProcessor.GetMesh(proxy);
            
            var modifiedMesh = FlipMeshProcessor.FlipMesh(mesh, component.FlipX, component.FlipY, component.FlipZ);

            return Task.FromResult<IRenderFilterNode>(new FlipMeshPreviewNode(modifiedMesh));
        }
    }

    internal class FlipMeshPreviewNode : IRenderFilterNode
    {
        public RenderAspects WhatChanged => RenderAspects.Mesh;
        private Mesh _modifiedMesh; 

        public FlipMeshPreviewNode(Mesh modifiedMesh)
        {
            _modifiedMesh = modifiedMesh;
        }
        
        public void OnFrame(Renderer original, Renderer proxy)
        {
            FlipMeshProcessor.AssignMesh(proxy, _modifiedMesh);
        }

    }
}

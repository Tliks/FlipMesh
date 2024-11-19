using nadena.dev.ndmf;
using com.aoyon.flip_mesh;

[assembly: ExportsPlugin(typeof(PluginDefinition))]

namespace com.aoyon.flip_mesh
{
    public class PluginDefinition : Plugin<PluginDefinition>
    {
        public override string QualifiedName => "com.aoyon.flip_mesh";

        public override string DisplayName => "Flip Mesh";

        protected override void Configure()
        {
            // Todo: 干渉するプラグインの調査
            var sequence =
                InPhase(BuildPhase.Transforming);

            sequence
            .Run(FlipMeshPass.Instance)
            .PreviewingWith(new FlipMeshPreview());
        }
    }
}
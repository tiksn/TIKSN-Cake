using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace TIKSN.Cake.Addin
{
    [CakeAliasCategory("Sample")]
    public static class AddinAliases
    {
        [CakeMethodAlias]
        public static void Hello(this ICakeContext ctx, string name)
        {
            ctx.Log.Information("Hello " + name);
        }
    }
}
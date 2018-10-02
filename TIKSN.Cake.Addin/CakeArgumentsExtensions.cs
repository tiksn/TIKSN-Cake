using Cake.Core;

namespace TIKSN.Cake.Addin
{
    public static class CakeArgumentsExtensions
    {
        public static string GetNullableArgument(this ICakeArguments cakeArguments, string name)
        {
            if (!cakeArguments.HasArgument(name))
                return null;

            return cakeArguments.GetArgument(name);
        }
    }
}

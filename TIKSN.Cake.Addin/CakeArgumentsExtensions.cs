using Cake.Core;

namespace TIKSN.Cake.Addin
{
    public static class CakeArgumentsExtensions
    {
        public static string GetArgumentOrDefault(this ICakeArguments cakeArguments, string name)
        {
            if (!cakeArguments.HasArgument(name))
                return string.Empty;

            return cakeArguments.GetArgument(name);
        }
    }
}

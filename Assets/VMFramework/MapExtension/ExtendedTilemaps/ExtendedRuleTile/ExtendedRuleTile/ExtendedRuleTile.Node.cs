using VMFramework.Core;

namespace VMFramework.Maps
{
    public partial class ExtendedRuleTile : IParentProvider<ExtendedRuleTile>
    {
        ExtendedRuleTile IParentProvider<ExtendedRuleTile>.GetParent() => parentRuleTile;
    }
}
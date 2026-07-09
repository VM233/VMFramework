using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public interface IStateCloner
    {
        public void CloneFrom(IStateCloner cloner, StateCloneHint hint);
    }
}
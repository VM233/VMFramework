using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public interface IStateCloneable
    {
        public int ClonePriority => PriorityDefines.MEDIUM;
        
        public void CloneFrom(IStateCloneable stateCloneable, StateCloneHint hint);
    }
}
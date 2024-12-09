using System;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public class InputsAndOutputsContainer : Container, IInputsContainer, IOutputsContainer
    {
        protected InputsAndOutputsContainerConfig InputsAndOutputsContainerConfig =>
            (InputsAndOutputsContainerConfig)GamePrefab;

        public RangeInteger InputsRange { get; private set; }

        public RangeInteger OutputsRange { get; private set; }

        RangeInteger IInputsContainer.InputsRange => InputsRange;

        RangeInteger IOutputsContainer.OutputsRange => OutputsRange;

        protected override void OnCreate()
        {
            base.OnCreate();

            InputsRange = new(InputsAndOutputsContainerConfig.inputsRange);
            OutputsRange = new(InputsAndOutputsContainerConfig.outputsRange);
        }

        #region Add

        public override bool TryAddItem(IContainerItem item, int preferredCount, out int addedCount)
        {
            return TryAddItem(item, preferredCount, InputsRange.min, InputsRange.max, out addedCount);
        }

        #endregion

        #region Sort & Stack

        public override void StackItems()
        {
            this.StackItems(InputsRange.min, InputsRange.max);
            this.StackItems(OutputsRange.min, OutputsRange.max);
        }

        public override void Sort(Comparison<IContainerItem> comparison)
        {
            Sort(comparison, InputsRange.min, InputsRange.max);
            Sort(comparison, OutputsRange.min, OutputsRange.max);
        }

        #endregion
    }
}

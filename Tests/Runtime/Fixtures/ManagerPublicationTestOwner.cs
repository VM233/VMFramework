using System;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public class ManagerPublicationTestOwner : ManagerBehaviour<IManagerPublicationTestOwner>,
        IManagerPublicationTestOwner
    {
        public Exception RetirementFailure { get; set; }

        protected override void OnDestroy()
        {
            try
            {
                if (RetirementFailure != null) throw RetirementFailure;
            }
            finally
            {
                base.OnDestroy();
            }
        }
    }
}

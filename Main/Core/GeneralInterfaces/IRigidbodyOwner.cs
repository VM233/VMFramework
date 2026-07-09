using UnityEngine;

namespace VMFramework.Core
{
    public interface IRigidbodyOwner
    {
        public Rigidbody Rigidbody { get; }
    }
}
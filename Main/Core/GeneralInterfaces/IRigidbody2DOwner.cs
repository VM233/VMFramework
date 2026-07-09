using UnityEngine;

namespace VMFramework.Core
{
    public interface IRigidbody2DOwner
    {
        public Rigidbody2D Rigidbody2D { get; }
    }
}
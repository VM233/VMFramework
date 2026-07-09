using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Core
{
    /// <summary>
    /// 普通的单例MonoBehaviour，注意Awake时的线程安全问题
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DisallowMultipleComponent]
    public abstract class UniqueMonoBehaviour<T> : SerializedMonoBehaviour
        where T : UniqueMonoBehaviour<T>
    {
        public static T Instance { get; set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                throw new Exception($"重复添加组件{nameof(T)}");
            }

            Instance = (T)this;
        }
    }
}
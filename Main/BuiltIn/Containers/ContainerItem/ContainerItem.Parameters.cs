using VMFramework.Parameters;

namespace VMFramework.Containers
{
    [ParameterDefine(COUNT_PARAMETER, typeof(int))]
    [ParameterDefine(COUNT_FLOAT_PARAMETER, typeof(float))]
    public abstract partial class ContainerItem : IParameterProvider
    {
        public const string COUNT_PARAMETER = "Count";
        public const string COUNT_FLOAT_PARAMETER = "Count Float";

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == COUNT_PARAMETER)
            {
                if (countProperty.GetValue() is TValue validValue)
                {
                    value = validValue;
                    return true;
                }
            }
            else if (key == COUNT_FLOAT_PARAMETER)
            {
                if ((float)countProperty.GetValue() is TValue validValue)
                {
                    value = validValue;
                    return true;
                }
            }

            return false;
        }
    }
}
using VMFramework.Core;

namespace VMFramework.UI
{
    public readonly struct SimpleText
    {
        public readonly string text;
        
        public SimpleText(string text)
        {
            this.text = text;
        }

        public SimpleText(int value)
        {
            text = value.ToString();
        }

        public SimpleText(float value, int decimalPlaces = 0)
        {
            text = value.ToString(decimalPlaces);
        }
        
        public static implicit operator SimpleText(string text)
        {
            return new SimpleText(text);
        }

        public static implicit operator SimpleText(int value)
        {
            return new SimpleText(value);
        }

        public static implicit operator SimpleText(float value)
        {
            return new SimpleText(value);
        }
    }
}
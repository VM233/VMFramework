namespace VMFramework.Configuration
{
    public struct CommonPresetEntry<TValue>
    {
        public string presetName;
        public TValue value;
        
        public CommonPresetEntry(string presetName, TValue value)
        {
            this.presetName = presetName;
            this.value = value;
        }
    }
}
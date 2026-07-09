namespace VMFramework.OdinExtensions
{
    public sealed class KeyNameAttribute : GeneralValueDropdownAttribute
    {
        public readonly string TableName;

        public KeyNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
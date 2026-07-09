#if UNITY_EDITOR
namespace VMFramework.Editor.BatchProcessor
{
    public static class BatchProcessorNames
    {
        public const string BATCH_PROCESSOR_NAME = "Batch Processor";
        
        public const string BATCH_PROCESS_NAME = "Batch Process";
        
        public const string BATCH_PROCESS_PATH = BATCH_PROCESSOR_NAME + "/" + BATCH_PROCESS_NAME;
        
        public const string ADD_TO_BATCH_PROCESSOR_NAME = "Add To Batch Processor";
        
        public const string ADD_TO_BATCH_PROCESSOR_PATH =
            BATCH_PROCESSOR_NAME + "/" + ADD_TO_BATCH_PROCESSOR_NAME;

        public const string ADD_ALL_GLOBAL_SETTINGS_FILE_TO_BATCH_PROCESSOR_NAME =
            "Add All Global Settings File To Batch Processor";
        
        public const string ADD_ALL_GLOBAL_SETTINGS_FILE_TO_BATCH_PROCESSOR_PATH =
            BATCH_PROCESSOR_NAME + "/" + ADD_ALL_GLOBAL_SETTINGS_FILE_TO_BATCH_PROCESSOR_NAME;
        
        public const string ADD_ALL_PREFABS_TO_BATCH_PROCESSOR_NAME = "Add All Prefabs To Batch Processor";
        
        public const string ADD_ALL_PREFABS_TO_BATCH_PROCESSOR_PATH =
            BATCH_PROCESSOR_NAME + "/" + ADD_ALL_PREFABS_TO_BATCH_PROCESSOR_NAME;
    }
}
#endif
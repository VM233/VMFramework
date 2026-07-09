using Newtonsoft.Json;

namespace VMFramework.Configuration
{
    [JsonObject(MemberSerialization.OptIn, ItemTypeNameHandling = TypeNameHandling.All)]
    public abstract partial class BaseConfig : IConfig
    {
        public bool InitDone { get; private set; } = false;

        public virtual void CheckSettings()
        {

        }

        public void Init()
        {
            OnInit();

            InitDone = true;
        }

        protected virtual void OnInit()
        {

        }
    }
}

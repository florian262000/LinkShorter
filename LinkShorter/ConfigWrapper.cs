using Newtonsoft.Json.Linq;

namespace LinkShorter
{
    public class ConfigWrapper
    {
        private JObject config;

        public ConfigWrapper(JObject config)
        {
            this.config = config;
        }

        public JObject Get()
        {
            return this.config;
        }
    }
}
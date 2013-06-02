using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components;

namespace PhpVH.ScanPlugins
{
    public abstract class ConfigurableScanPluginBase<TConfig> : ScanPluginBase
        where TConfig : class
    {
        public TConfig Config { get; private set; }

        public ConfigurableScanPluginBase()
            : base()
        {
        }

        protected TConfig DeserializeConfig()
        {
            var filename = ".\\ScanPlugins\\" + GetType().Name + ".xml";

            if (File.Exists(filename))
            {
                var serializer = new XmlSerializer(typeof(TConfig));
                return serializer.Deserialize(filename) as TConfig;
            }
            else
                return null;
        }

        protected abstract void InitializeCore();

        public override void Initialize()
        {
            Config = DeserializeConfig();
            InitializeCore();

            base.Initialize();
        }
    }
}

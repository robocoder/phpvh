using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Components;
using Components.Aphid.Interpreter;
using System.Text.RegularExpressions;

namespace PhpVH.ScanPlugins
{
    public abstract class ScriptableScanPluginBase : ScanPluginBase
    {
        protected AphidObject ScriptResult { get; set; }

        protected bool WasScriptCached { get; private set; }

        public ScriptableScanPluginBase()
            : base()
        {
        }

        protected AphidObject ExecuteScript()
        {
            string name = GetType().Name;
            name = Regex.Replace(name, @"ScanPlugin$", "", RegexOptions.IgnoreCase);

            var filename =  ".\\ScanPlugins\\" + name + ".alx";

            WasScriptCached = ScriptLauncher.IsExecuteCached(filename);

            return ScriptLauncher.Execute(filename);
        }

        protected abstract void InitializeCore();

        public override void Initialize()
        {
            ScriptResult = ExecuteScript();
            InitializeCore();
        }
    }

    public abstract class ScriptableScanPluginBase<TConfig> : ScriptableScanPluginBase
        where TConfig : class, new()
    {
        public TConfig Config { get; protected set; }

        public override void Initialize()
        {
            ScriptResult = ExecuteScript();
            Config = new TConfig();
            ScriptResult.Bind(Config);
            InitializeCore();
        }
    }
}

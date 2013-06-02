using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoBuildScriptGenerator
{
    public abstract class BuildScriptGenerator
    {
        public abstract string CreateScript(Project project);
    }
}

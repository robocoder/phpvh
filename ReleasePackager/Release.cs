using Components;
using Components.Aphid.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleasePackager
{
    public class Release : IAphidBindable
    {
        [AphidProperty("root")]
        public string Root { get; set; }

        [AphidProperty("output")]
        public string Output { get; set; }

        [AphidProperty("mainProject")]
        public string MainProject { get; set; }

        public ReleaseProject[] Projects { get; set; }

        [AphidProperty("cleanup")]
        public Cleanup Cleanup { get; set; }

        public void OnBinding(AphidObject source)
        {

        }

        public void OnBound(AphidObject source)
        {
            var p = source["projects"]
                .GetStringList()
                .Select(x => new
                {
                    Name = x,
                    Project = ReleaseProject.Load(PathHelper.GetExecutingPath(Root, x), "debug")
                })
                .ToArray();

            p.Single(x => x.Name == MainProject).Project.IsMainProject = true;
            Projects = p.Select(x => x.Project).ToArray();
        }
    }
}

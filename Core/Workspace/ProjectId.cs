using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Core.Workspace
{
    [DataContract(Name = "ProjectId", Namespace = "http://davenport.edu")]
    public class CoreProjectId
    {
        public CoreProjectId()
        {
            this.Id = Guid.NewGuid();
        }

        public CoreProjectId(string path, string name) : this(Guid.NewGuid(), path, name)
        {

        }

        public CoreProjectId(Guid id, string path, string name)
        {
            this.Id = id;
            this.Path = path;
            this.Name = name;
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}

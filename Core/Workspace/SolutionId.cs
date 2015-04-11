using System;
using System.Runtime.Serialization;

namespace Core.Workspace
{
    [DataContract(Name = "SolutionId", Namespace = "http://davenport.edu")]
    public class SolutionId
    {
        public SolutionId()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Create a new SolutionId
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public SolutionId(string path, string name) : this(Guid.NewGuid(), path, name)
        {

        }

        public SolutionId(Guid id, string path, string name)
        {
            Id = id;
            Path = path;
            Name = name;
        }

        /// <summary>
        /// Identifier
        /// </summary>
        /// <returns>Guid</returns>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// The root directory of a solution
        /// </summary>
        /// <returns>string</returns>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// The name of the solution
        /// </summary>
        /// <returns>string</returns>
        [DataMember]
        public string Name { get; set; }
    }
}

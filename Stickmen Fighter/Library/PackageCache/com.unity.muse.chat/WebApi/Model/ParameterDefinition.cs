using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenAPIDateConverter = Unity.Muse.Chat.BackendApi.Client.OpenAPIDateConverter;

namespace Unity.Muse.Chat.BackendApi.Model
{
    /// <summary>
    /// ParameterDefinition
    /// </summary>
    [DataContract(Name = "ParameterDefinition")]
    internal partial class ParameterDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDefinition" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ParameterDefinition() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter (required)</param>
        /// <param name="type">The parameters type, in the form of the origin language. I.E. functions originating from Unity should be C# types. (required)</param>
        /// <param name="description">A description of the parameter used by the LLM (required)</param>
        public ParameterDefinition(string name = default(string), string type = default(string), string description = default(string))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for ParameterDefinition and cannot be null");
            }
            this.Name = name;
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for ParameterDefinition and cannot be null");
            }
            this.Type = type;
            // to ensure "description" is required (not null)
            if (description == null)
            {
                throw new ArgumentNullException("description is a required property for ParameterDefinition and cannot be null");
            }
            this.Description = description;
        }

        /// <summary>
        /// The name of the parameter
        /// </summary>
        /// <value>The name of the parameter</value>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// The parameters type, in the form of the origin language. I.E. functions originating from Unity should be C# types.
        /// </summary>
        /// <value>The parameters type, in the form of the origin language. I.E. functions originating from Unity should be C# types.</value>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public string Type { get; set; }

        /// <summary>
        /// A description of the parameter used by the LLM
        /// </summary>
        /// <value>A description of the parameter used by the LLM</value>
        [DataMember(Name = "description", IsRequired = true, EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ParameterDefinition {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

    }

}

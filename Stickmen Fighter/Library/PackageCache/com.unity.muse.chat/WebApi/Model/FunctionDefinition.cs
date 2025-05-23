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
    /// FunctionDefinition
    /// </summary>
    [DataContract(Name = "FunctionDefinition")]
    internal partial class FunctionDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDefinition" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected FunctionDefinition() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the function to be called. This should be the original function name and should not be converted to pythonic snake case. (required)</param>
        /// <param name="description">The description of the function, used by the LLM. (required)</param>
        /// <param name="parameters">The parameters of the function.</param>
        /// <param name="tags">tags</param>
        public FunctionDefinition(string name = default(string), string description = default(string), List<ParameterDefinition> parameters = default(List<ParameterDefinition>), List<string> tags = default(List<string>))
        {
            // to ensure "name" is required (not null)
            if (name == null)
            {
                throw new ArgumentNullException("name is a required property for FunctionDefinition and cannot be null");
            }
            this.Name = name;
            // to ensure "description" is required (not null)
            if (description == null)
            {
                throw new ArgumentNullException("description is a required property for FunctionDefinition and cannot be null");
            }
            this.Description = description;
            this.Parameters = parameters;
            this.Tags = tags;
        }

        /// <summary>
        /// The name of the function to be called. This should be the original function name and should not be converted to pythonic snake case.
        /// </summary>
        /// <value>The name of the function to be called. This should be the original function name and should not be converted to pythonic snake case.</value>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        /// The description of the function, used by the LLM.
        /// </summary>
        /// <value>The description of the function, used by the LLM.</value>
        [DataMember(Name = "description", IsRequired = true, EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// The parameters of the function.
        /// </summary>
        /// <value>The parameters of the function.</value>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        public List<ParameterDefinition> Parameters { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class FunctionDefinition {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Parameters: ").Append(Parameters).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
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

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
    /// FunctionCall
    /// </summary>
    [DataContract(Name = "FunctionCall")]
    internal partial class FunctionCall
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionCall" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected FunctionCall() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionCall" /> class.
        /// </summary>
        /// <param name="function">function (required)</param>
        /// <param name="parameters">parameters (required)</param>
        public FunctionCall(string function = default(string), List<string> parameters = default(List<string>))
        {
            // to ensure "function" is required (not null)
            if (function == null)
            {
                throw new ArgumentNullException("function is a required property for FunctionCall and cannot be null");
            }
            this.Function = function;
            // to ensure "parameters" is required (not null)
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters is a required property for FunctionCall and cannot be null");
            }
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets or Sets Function
        /// </summary>
        [DataMember(Name = "function", IsRequired = true, EmitDefaultValue = true)]
        public string Function { get; set; }

        /// <summary>
        /// Gets or Sets Parameters
        /// </summary>
        [DataMember(Name = "parameters", IsRequired = true, EmitDefaultValue = true)]
        public List<string> Parameters { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class FunctionCall {\n");
            sb.Append("  Function: ").Append(Function).Append("\n");
            sb.Append("  Parameters: ").Append(Parameters).Append("\n");
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

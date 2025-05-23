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
    /// CreateConversationRequest, post resources that need to be cached per conversation and get a conversation_id.
    /// </summary>
    [DataContract(Name = "CreateConversationRequest")]
    internal partial class CreateConversationRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConversationRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateConversationRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateConversationRequest" /> class.
        /// </summary>
        /// <param name="organizationId">The ID of the Unity organization. (required)</param>
        /// <param name="functionCatalog">functionCatalog</param>
        public CreateConversationRequest(string organizationId = default(string), List<FunctionDefinition> functionCatalog = default(List<FunctionDefinition>))
        {
            // to ensure "organizationId" is required (not null)
            if (organizationId == null)
            {
                throw new ArgumentNullException("organizationId is a required property for CreateConversationRequest and cannot be null");
            }
            this.OrganizationId = organizationId;
            this.FunctionCatalog = functionCatalog;
        }

        /// <summary>
        /// The ID of the Unity organization.
        /// </summary>
        /// <value>The ID of the Unity organization.</value>
        [DataMember(Name = "organization_id", IsRequired = true, EmitDefaultValue = true)]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Gets or Sets FunctionCatalog
        /// </summary>
        [DataMember(Name = "function_catalog", EmitDefaultValue = true)]
        public List<FunctionDefinition> FunctionCatalog { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateConversationRequest {\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  FunctionCatalog: ").Append(FunctionCatalog).Append("\n");
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

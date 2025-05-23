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
    /// Action code repair request, to interact with Muse Agent.
    /// </summary>
    [DataContract(Name = "ActionCodeRepairRequest")]
    internal partial class ActionCodeRepairRequest
    {

        /// <summary>
        /// Type of script for repairing.
        /// </summary>
        /// <value>Type of script for repairing.</value>
        [DataMember(Name = "script_type", EmitDefaultValue = false)]
        public ScriptType? ScriptType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCodeRepairRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ActionCodeRepairRequest() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCodeRepairRequest" /> class.
        /// </summary>
        /// <param name="conversationId">Uniform conversation ID.</param>
        /// <param name="streamResponse">Whether to stream Muse Chat response. (required)</param>
        /// <param name="organizationId">The ID of the Unity organization. (required)</param>
        /// <param name="messageIndex">Message index. (required)</param>
        /// <param name="userPrompt">userPrompt</param>
        /// <param name="errorToRepair">The csharp script error to repair. (required)</param>
        /// <param name="scriptToRepair">The csharp script to repair. (required)</param>
        /// <param name="scriptType">Type of script for repairing.</param>
        /// <param name="tags">List of tags associated with chat request</param>
        /// <param name="extraBody">extraBody</param>
        /// <param name="debug">debug</param>
        /// <param name="unityVersion">unityVersion</param>
        public ActionCodeRepairRequest(string conversationId = default(string), bool streamResponse = default(bool), string organizationId = default(string), int messageIndex = default(int), string userPrompt = default(string), string errorToRepair = default(string), string scriptToRepair = default(string), ScriptType? scriptType = default(ScriptType?), List<string> tags = default(List<string>), Object extraBody = default(Object), bool? debug = default(bool?), string unityVersion = default(string))
        {
            this.StreamResponse = streamResponse;
            // to ensure "organizationId" is required (not null)
            if (organizationId == null)
            {
                throw new ArgumentNullException("organizationId is a required property for ActionCodeRepairRequest and cannot be null");
            }
            this.OrganizationId = organizationId;
            this.MessageIndex = messageIndex;
            // to ensure "errorToRepair" is required (not null)
            if (errorToRepair == null)
            {
                throw new ArgumentNullException("errorToRepair is a required property for ActionCodeRepairRequest and cannot be null");
            }
            this.ErrorToRepair = errorToRepair;
            // to ensure "scriptToRepair" is required (not null)
            if (scriptToRepair == null)
            {
                throw new ArgumentNullException("scriptToRepair is a required property for ActionCodeRepairRequest and cannot be null");
            }
            this.ScriptToRepair = scriptToRepair;
            this.ConversationId = conversationId;
            this.UserPrompt = userPrompt;
            this.ScriptType = scriptType;
            this.Tags = tags;
            this.ExtraBody = extraBody;
            this.Debug = debug;
            this.UnityVersion = unityVersion;
        }

        /// <summary>
        /// Uniform conversation ID.
        /// </summary>
        /// <value>Uniform conversation ID.</value>
        [DataMember(Name = "conversation_id", EmitDefaultValue = false)]
        public string ConversationId { get; set; }

        /// <summary>
        /// Whether to stream Muse Chat response.
        /// </summary>
        /// <value>Whether to stream Muse Chat response.</value>
        [DataMember(Name = "stream_response", IsRequired = true, EmitDefaultValue = true)]
        public bool StreamResponse { get; set; }

        /// <summary>
        /// The ID of the Unity organization.
        /// </summary>
        /// <value>The ID of the Unity organization.</value>
        [DataMember(Name = "organization_id", IsRequired = true, EmitDefaultValue = true)]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Message index.
        /// </summary>
        /// <value>Message index.</value>
        [DataMember(Name = "message_index", IsRequired = true, EmitDefaultValue = true)]
        public int MessageIndex { get; set; }

        /// <summary>
        /// Gets or Sets UserPrompt
        /// </summary>
        [DataMember(Name = "user_prompt", EmitDefaultValue = true)]
        public string UserPrompt { get; set; }

        /// <summary>
        /// The csharp script error to repair.
        /// </summary>
        /// <value>The csharp script error to repair.</value>
        [DataMember(Name = "error_to_repair", IsRequired = true, EmitDefaultValue = true)]
        public string ErrorToRepair { get; set; }

        /// <summary>
        /// The csharp script to repair.
        /// </summary>
        /// <value>The csharp script to repair.</value>
        [DataMember(Name = "script_to_repair", IsRequired = true, EmitDefaultValue = true)]
        public string ScriptToRepair { get; set; }

        /// <summary>
        /// List of tags associated with chat request
        /// </summary>
        /// <value>List of tags associated with chat request</value>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets ExtraBody
        /// </summary>
        [DataMember(Name = "extra_body", EmitDefaultValue = true)]
        public Object ExtraBody { get; set; }

        /// <summary>
        /// Gets or Sets Debug
        /// </summary>
        [DataMember(Name = "debug", EmitDefaultValue = true)]
        public bool? Debug { get; set; }

        /// <summary>
        /// Gets or Sets UnityVersion
        /// </summary>
        [DataMember(Name = "unity_version", EmitDefaultValue = true)]
        public string UnityVersion { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ActionCodeRepairRequest {\n");
            sb.Append("  ConversationId: ").Append(ConversationId).Append("\n");
            sb.Append("  StreamResponse: ").Append(StreamResponse).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  MessageIndex: ").Append(MessageIndex).Append("\n");
            sb.Append("  UserPrompt: ").Append(UserPrompt).Append("\n");
            sb.Append("  ErrorToRepair: ").Append(ErrorToRepair).Append("\n");
            sb.Append("  ScriptToRepair: ").Append(ScriptToRepair).Append("\n");
            sb.Append("  ScriptType: ").Append(ScriptType).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  ExtraBody: ").Append(ExtraBody).Append("\n");
            sb.Append("  Debug: ").Append(Debug).Append("\n");
            sb.Append("  UnityVersion: ").Append(UnityVersion).Append("\n");
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

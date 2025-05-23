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
    /// Subset of conversation fields relevant to clients, excluding data that is used by other server-side components.
    /// </summary>
    [DataContract(Name = "ClientConversation")]
    internal partial class ClientConversation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConversation" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ClientConversation() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientConversation" /> class.
        /// </summary>
        /// <param name="id">Uniform conversation ID.</param>
        /// <param name="title">Title of conversation. (required)</param>
        /// <param name="history">Conversation history. (required)</param>
        /// <param name="owners">User IDs of owners of the conversation. (required)</param>
        /// <param name="tags">tags</param>
        /// <param name="context">context</param>
        /// <param name="isFavorite">isFavorite</param>
        public ClientConversation(string id = default(string), string title = default(string), List<ConversationFragment> history = default(List<ConversationFragment>), List<string> owners = default(List<string>), List<string> tags = default(List<string>), string context = default(string), bool? isFavorite = default(bool?))
        {
            // to ensure "title" is required (not null)
            if (title == null)
            {
                throw new ArgumentNullException("title is a required property for ClientConversation and cannot be null");
            }
            this.Title = title;
            // to ensure "history" is required (not null)
            if (history == null)
            {
                throw new ArgumentNullException("history is a required property for ClientConversation and cannot be null");
            }
            this.History = history;
            // to ensure "owners" is required (not null)
            if (owners == null)
            {
                throw new ArgumentNullException("owners is a required property for ClientConversation and cannot be null");
            }
            this.Owners = owners;
            this.Id = id;
            this.Tags = tags;
            this.Context = context;
            this.IsFavorite = isFavorite;
        }

        /// <summary>
        /// Uniform conversation ID.
        /// </summary>
        /// <value>Uniform conversation ID.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Title of conversation.
        /// </summary>
        /// <value>Title of conversation.</value>
        [DataMember(Name = "title", IsRequired = true, EmitDefaultValue = true)]
        public string Title { get; set; }

        /// <summary>
        /// Conversation history.
        /// </summary>
        /// <value>Conversation history.</value>
        [DataMember(Name = "history", IsRequired = true, EmitDefaultValue = true)]
        public List<ConversationFragment> History { get; set; }

        /// <summary>
        /// User IDs of owners of the conversation.
        /// </summary>
        /// <value>User IDs of owners of the conversation.</value>
        [DataMember(Name = "owners", IsRequired = true, EmitDefaultValue = true)]
        public List<string> Owners { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets Context
        /// </summary>
        [DataMember(Name = "context", EmitDefaultValue = true)]
        public string Context { get; set; }

        /// <summary>
        /// Gets or Sets IsFavorite
        /// </summary>
        [DataMember(Name = "is_favorite", EmitDefaultValue = true)]
        public bool? IsFavorite { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ClientConversation {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  History: ").Append(History).Append("\n");
            sb.Append("  Owners: ").Append(Owners).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Context: ").Append(Context).Append("\n");
            sb.Append("  IsFavorite: ").Append(IsFavorite).Append("\n");
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

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
    /// Model of fundamental conversation information.
    /// </summary>
    [DataContract(Name = "ConversationInfo")]
    internal partial class ConversationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationInfo" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ConversationInfo() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationInfo" /> class.
        /// </summary>
        /// <param name="conversationId">Uniform conversation ID. (required)</param>
        /// <param name="title">Conversation title. (required)</param>
        /// <param name="lastMessageTimestamp">UTC milliseconds timestamp of last message in conversation. (required)</param>
        /// <param name="tags">tags</param>
        /// <param name="isFavorite">isFavorite</param>
        public ConversationInfo(string conversationId = default(string), string title = default(string), long lastMessageTimestamp = default(long), List<string> tags = default(List<string>), bool? isFavorite = default(bool?))
        {
            // to ensure "conversationId" is required (not null)
            if (conversationId == null)
            {
                throw new ArgumentNullException("conversationId is a required property for ConversationInfo and cannot be null");
            }
            this.ConversationId = conversationId;
            // to ensure "title" is required (not null)
            if (title == null)
            {
                throw new ArgumentNullException("title is a required property for ConversationInfo and cannot be null");
            }
            this.Title = title;
            this.LastMessageTimestamp = lastMessageTimestamp;
            this.Tags = tags;
            this.IsFavorite = isFavorite;
        }

        /// <summary>
        /// Uniform conversation ID.
        /// </summary>
        /// <value>Uniform conversation ID.</value>
        [DataMember(Name = "conversation_id", IsRequired = true, EmitDefaultValue = true)]
        public string ConversationId { get; set; }

        /// <summary>
        /// Conversation title.
        /// </summary>
        /// <value>Conversation title.</value>
        [DataMember(Name = "title", IsRequired = true, EmitDefaultValue = true)]
        public string Title { get; set; }

        /// <summary>
        /// UTC milliseconds timestamp of last message in conversation.
        /// </summary>
        /// <value>UTC milliseconds timestamp of last message in conversation.</value>
        [DataMember(Name = "last_message_timestamp", IsRequired = true, EmitDefaultValue = true)]
        public long LastMessageTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets Tags
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public List<string> Tags { get; set; }

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
            sb.Append("class ConversationInfo {\n");
            sb.Append("  ConversationId: ").Append(ConversationId).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  LastMessageTimestamp: ").Append(LastMessageTimestamp).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
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

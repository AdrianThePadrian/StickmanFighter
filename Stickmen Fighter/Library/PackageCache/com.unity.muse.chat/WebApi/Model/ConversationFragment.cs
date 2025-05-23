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
    /// Some people might call this a message.
    /// </summary>
    [DataContract(Name = "ConversationFragment")]
    internal partial class ConversationFragment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationFragment" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ConversationFragment() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationFragment" /> class.
        /// </summary>
        /// <param name="id">Uniform message ID.</param>
        /// <param name="requestId">requestId</param>
        /// <param name="timestamp">UTC milliseconds timestamp of message ... I mean ... conversation fragment.</param>
        /// <param name="role">Role of conversation fragment author, either \&quot;user\&quot; or \&quot;assistant\&quot;. (required)</param>
        /// <param name="content">Content of conversation fragment. (required)</param>
        /// <param name="author">User ID of conversation fragment author. (required)</param>
        /// <param name="tags">List of tags associated with the conversation fragment.</param>
        /// <param name="preferred">preferred</param>
        public ConversationFragment(string id = default(string), string requestId = default(string), long timestamp = default(long), string role = default(string), string content = default(string), string author = default(string), List<string> tags = default(List<string>), bool? preferred = default(bool?))
        {
            // to ensure "role" is required (not null)
            if (role == null)
            {
                throw new ArgumentNullException("role is a required property for ConversationFragment and cannot be null");
            }
            this.Role = role;
            // to ensure "content" is required (not null)
            if (content == null)
            {
                throw new ArgumentNullException("content is a required property for ConversationFragment and cannot be null");
            }
            this.Content = content;
            // to ensure "author" is required (not null)
            if (author == null)
            {
                throw new ArgumentNullException("author is a required property for ConversationFragment and cannot be null");
            }
            this.Author = author;
            this.Id = id;
            this.RequestId = requestId;
            this.Timestamp = timestamp;
            this.Tags = tags;
            this.Preferred = preferred;
        }

        /// <summary>
        /// Uniform message ID.
        /// </summary>
        /// <value>Uniform message ID.</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [DataMember(Name = "request_id", EmitDefaultValue = true)]
        public string RequestId { get; set; }

        /// <summary>
        /// UTC milliseconds timestamp of message ... I mean ... conversation fragment.
        /// </summary>
        /// <value>UTC milliseconds timestamp of message ... I mean ... conversation fragment.</value>
        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        public long Timestamp { get; set; }

        /// <summary>
        /// Role of conversation fragment author, either \&quot;user\&quot; or \&quot;assistant\&quot;.
        /// </summary>
        /// <value>Role of conversation fragment author, either \&quot;user\&quot; or \&quot;assistant\&quot;.</value>
        [DataMember(Name = "role", IsRequired = true, EmitDefaultValue = true)]
        public string Role { get; set; }

        /// <summary>
        /// Content of conversation fragment.
        /// </summary>
        /// <value>Content of conversation fragment.</value>
        [DataMember(Name = "content", IsRequired = true, EmitDefaultValue = true)]
        public string Content { get; set; }

        /// <summary>
        /// User ID of conversation fragment author.
        /// </summary>
        /// <value>User ID of conversation fragment author.</value>
        [DataMember(Name = "author", IsRequired = true, EmitDefaultValue = true)]
        public string Author { get; set; }

        /// <summary>
        /// List of tags associated with the conversation fragment.
        /// </summary>
        /// <value>List of tags associated with the conversation fragment.</value>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or Sets Preferred
        /// </summary>
        [DataMember(Name = "preferred", EmitDefaultValue = true)]
        public bool? Preferred { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ConversationFragment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RequestId: ").Append(RequestId).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Role: ").Append(Role).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  Author: ").Append(Author).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Preferred: ").Append(Preferred).Append("\n");
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

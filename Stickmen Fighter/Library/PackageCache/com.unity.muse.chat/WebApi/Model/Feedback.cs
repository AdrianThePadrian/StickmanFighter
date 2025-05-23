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
    /// Feedback request, with feedback.
    /// </summary>
    [DataContract(Name = "Feedback")]
    internal partial class Feedback
    {

        /// <summary>
        /// Explicit feedback sentiment, either \&quot;positive\&quot; or \&quot;negative\&quot;.
        /// </summary>
        /// <value>Explicit feedback sentiment, either \&quot;positive\&quot; or \&quot;negative\&quot;.</value>
        [DataMember(Name = "sentiment", IsRequired = true, EmitDefaultValue = true)]
        public Sentiment Sentiment { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [DataMember(Name = "category", IsRequired = true, EmitDefaultValue = true)]
        public Category Category { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Feedback" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected Feedback() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Feedback" /> class.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="creationDateUtc">creationDateUtc</param>
        /// <param name="conversationId">Uniform conversation ID. (required)</param>
        /// <param name="conversationFragmentId">Conversation fragment ID. (required)</param>
        /// <param name="sentiment">Explicit feedback sentiment, either \&quot;positive\&quot; or \&quot;negative\&quot;. (required)</param>
        /// <param name="category">category (required)</param>
        /// <param name="details">details (required)</param>
        /// <param name="heapSessionUrl">heapSessionUrl</param>
        /// <param name="isUnityEmployee">isUnityEmployee</param>
        /// <param name="userId">userId</param>
        /// <param name="organizationId">The ID of the Unity organization. (required)</param>
        public Feedback(string id = default(string), DateTime creationDateUtc = default(DateTime), string conversationId = default(string), string conversationFragmentId = default(string), Sentiment sentiment = default(Sentiment), Category category = default(Category), string details = default(string), string heapSessionUrl = default(string), bool? isUnityEmployee = default(bool?), string userId = default(string), string organizationId = default(string))
        {
            // to ensure "conversationId" is required (not null)
            if (conversationId == null)
            {
                throw new ArgumentNullException("conversationId is a required property for Feedback and cannot be null");
            }
            this.ConversationId = conversationId;
            // to ensure "conversationFragmentId" is required (not null)
            if (conversationFragmentId == null)
            {
                throw new ArgumentNullException("conversationFragmentId is a required property for Feedback and cannot be null");
            }
            this.ConversationFragmentId = conversationFragmentId;
            this.Sentiment = sentiment;
            this.Category = category;
            // to ensure "details" is required (not null)
            if (details == null)
            {
                throw new ArgumentNullException("details is a required property for Feedback and cannot be null");
            }
            this.Details = details;
            // to ensure "organizationId" is required (not null)
            if (organizationId == null)
            {
                throw new ArgumentNullException("organizationId is a required property for Feedback and cannot be null");
            }
            this.OrganizationId = organizationId;
            this.Id = id;
            this.CreationDateUtc = creationDateUtc;
            this.HeapSessionUrl = heapSessionUrl;
            this.IsUnityEmployee = isUnityEmployee;
            this.UserId = userId;
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "_id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets CreationDateUtc
        /// </summary>
        [DataMember(Name = "creation_date_utc", EmitDefaultValue = false)]
        public DateTime CreationDateUtc { get; set; }

        /// <summary>
        /// Uniform conversation ID.
        /// </summary>
        /// <value>Uniform conversation ID.</value>
        [DataMember(Name = "conversation_id", IsRequired = true, EmitDefaultValue = true)]
        public string ConversationId { get; set; }

        /// <summary>
        /// Conversation fragment ID.
        /// </summary>
        /// <value>Conversation fragment ID.</value>
        [DataMember(Name = "conversation_fragment_id", IsRequired = true, EmitDefaultValue = true)]
        public string ConversationFragmentId { get; set; }

        /// <summary>
        /// Gets or Sets Details
        /// </summary>
        [DataMember(Name = "details", IsRequired = true, EmitDefaultValue = true)]
        public string Details { get; set; }

        /// <summary>
        /// Gets or Sets HeapSessionUrl
        /// </summary>
        [DataMember(Name = "heap_session_url", EmitDefaultValue = true)]
        public string HeapSessionUrl { get; set; }

        /// <summary>
        /// Gets or Sets IsUnityEmployee
        /// </summary>
        [DataMember(Name = "is_unity_employee", EmitDefaultValue = true)]
        public bool? IsUnityEmployee { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name = "user_id", EmitDefaultValue = true)]
        public string UserId { get; set; }

        /// <summary>
        /// The ID of the Unity organization.
        /// </summary>
        /// <value>The ID of the Unity organization.</value>
        [DataMember(Name = "organization_id", IsRequired = true, EmitDefaultValue = true)]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class Feedback {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  CreationDateUtc: ").Append(CreationDateUtc).Append("\n");
            sb.Append("  ConversationId: ").Append(ConversationId).Append("\n");
            sb.Append("  ConversationFragmentId: ").Append(ConversationFragmentId).Append("\n");
            sb.Append("  Sentiment: ").Append(Sentiment).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  Details: ").Append(Details).Append("\n");
            sb.Append("  HeapSessionUrl: ").Append(HeapSessionUrl).Append("\n");
            sb.Append("  IsUnityEmployee: ").Append(IsUnityEmployee).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
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

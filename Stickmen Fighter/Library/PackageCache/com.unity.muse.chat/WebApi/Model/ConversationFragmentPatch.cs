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
    /// AB test message preference request.
    /// </summary>
    [DataContract(Name = "ConversationFragmentPatch")]
    internal partial class ConversationFragmentPatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationFragmentPatch" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ConversationFragmentPatch() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationFragmentPatch" /> class.
        /// </summary>
        /// <param name="preferred">AB test message preference. (required)</param>
        public ConversationFragmentPatch(bool preferred = default(bool))
        {
            this.Preferred = preferred;
        }

        /// <summary>
        /// AB test message preference.
        /// </summary>
        /// <value>AB test message preference.</value>
        [DataMember(Name = "preferred", IsRequired = true, EmitDefaultValue = true)]
        public bool Preferred { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ConversationFragmentPatch {\n");
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

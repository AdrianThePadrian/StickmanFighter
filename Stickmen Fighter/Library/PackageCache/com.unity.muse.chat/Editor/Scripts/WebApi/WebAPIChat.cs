using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEngine.Networking;

#pragma warning disable CS0162 // Unreachable code detected

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        public MuseMessageUpdateHandler Chat(string prompt, string conversationID = "", string context = "")
        {
            if (!GetOrganizationID(out string organizationId))
            {
                throw new Exception("No valid organization found.");
            }

            ChatRequest options = new(
                prompt: prompt,
                context: string.IsNullOrWhiteSpace(context) ? null : context,
                streamResponse: true,
                conversationId: string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                organizationId: organizationId,
                dependencyInformation: UnityDataUtils.GetPackageMap(),
                projectSummary: UnityDataUtils.GetProjectSettingSummary(),
                unityVersions: k_UnityVersionField.ToList(),
                tags: new List<string>(new[] { UnityDataUtils.GetProjectId() }),
                extraBody: new Dictionary<string, object>
                {
                    { "enable_plugins", true },
                    { "muse_guard", true },
                    {
                        "mediation_system_prompt", string.IsNullOrWhiteSpace(MuseChatConstants.MediationPrompt)
                            ? null
                            : MuseChatConstants.MediationPrompt
                    },
                    { "skip_planning", MuseChatConstants.SkipPlanning }
                }
            );

            var updateHandler = new MuseMessageUpdateHandler(this);
            try
            {
                var api = BoostrapAPI(updateHandler.InterceptChatRequest, updateHandler.InterceptChatResponse, out var cancellationTokenSource);

                // Construct a wrapper object that groups important resources
                var activeChatRequestOperation = new ChatRequestOperation
                {
                    Options = options,
                    CancellationTokenSource = cancellationTokenSource,
                    ConversationId = conversationID
                };

                updateHandler.InitFromWebAPI(activeChatRequestOperation);

                // Start the request task, this makes the Intercept code
                // populate the MuseMessageUpdateHandler with the UnityWebRequest
                Task request = api.PostMuseChatV1Async(options, cancellationTokenSource.Token);

                // Add the task too
                activeChatRequestOperation.Task = request;

                updateHandler.Start();
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return updateHandler;
        }

        internal class ChatRequestOperation
        {
            public object Options;
            public Task Task;
            public UnityWebRequest WebRequest;
            public string ConversationId;
            public string AssistantMessageFragmentId;
            public string UserMessageFragmentId;
            public string FinalData;
            public bool IsComplete;
            public CancellationTokenSource CancellationTokenSource;
        }
    }
}

using UnityEditor;

namespace Unity.Muse.Chat
{
    internal static class MuseChatConstants
    {
        internal const int MaxInspirationCount = 100;
        internal const int MaxConversationHistory = 1000;
        internal const int MaxFeedbackMessageLength = 1000;
        internal const int MaxMuseMessageLength = 4000;

        internal const int CompactWindowThreshold = 700;
        internal const string CompactStyle = "mui-compact";

        internal const string TextCutoffSuffix = "...";

        internal const char UnityPathSeparator = '/';
        internal const string TemplateExtension = ".uxml";
        internal const string StyleExtension = ".uss";

        internal const string ResourceFolderName = "Resources";
        internal const string PackageName = "com.unity.muse.chat";
        internal const string PackageRoot = "";
        internal const string BasePath = "Packages/" + PackageName + PackageRoot + "/";
        internal const string UIEditorPath = "Editor/UI/";

        internal const string AssetFolder = "Assets/";
        internal const string ViewFolder = "Views/";
        internal const string StyleFolder = "Styles/";

        internal const string UIModulePath = BasePath + UIEditorPath;

        internal const string MuseChatBaseStyle = "MuseChat.tss";
        internal const string MuseChatSharedStyleDark = "MuseChatSharedDark";
        internal const string MuseChatSharedStyleLight = "MuseChatSharedLight";


        internal const string AppUIEditorClass = "unity-editor";
        internal const string AppUIThemeLight = "editor-light";
        internal const string AppUIThemeDark = "editor-dark";
        internal const string AppUIScale = "small";

        internal static readonly string SourcesReferenceColor = EditorGUIUtility.isProSkin ? "FF85ABFF" : "881f49FF";

        internal const string ProjectIdTagPrefix = "projId:";

        internal const string ContextTag = "#PROJECTCONTEXT#";
        internal static readonly string ContextTagEscaped = ContextTag.Replace("#", @"\#");

        internal const string DisclaimerText = @"// {0} AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

";

        internal const bool DebugMode = false;
        internal const string MediationPrompt = "";
        internal const bool SkipPlanning = false;

        internal const int SuggestedSelectedContextLimit = 5;
        internal const int PromptContextLimit = 17000;
    }
}

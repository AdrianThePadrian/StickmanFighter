using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using PoseAI;

public class RecordingUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startRecordingButton;
    public Button stopRecordingButton;
    public Button playButton;
    public Text instructionText;

    [Header("Recording Settings")]
    public PoseAICharacterAnimator poseAICharacter;
    public RuntimeAnimatorController player1AnimatorController;
    public RuntimeAnimatorController player2AnimatorController;
    private bool isRecording = false;
    private PoseAIAnimationRecorder recorder;
    public int currentPlayerIndex = 1;

    private string[] animationNames = { "HighAttack", "LowAttack", "Victory" };
    private int currentAnimationIndex = 0;

    private void Start()
    {
        recorder = FindFirstObjectByType<PoseAIAnimationRecorder>();

        if (recorder == null)
        {
            Debug.LogError("PoseAIAnimationRecorder not found in scene!");
            return;
        }

        if (poseAICharacter != null)
        {
            var animator = poseAICharacter.GetComponent<Animator>();
            animator.runtimeAnimatorController = player1AnimatorController;
            recorder.SetTargetAnimator(animator);
        }
        else
        {
            Debug.LogError("No PoseAI character assigned!");
        }

        SetupButtons();
        UpdateInstructionText();
    }

    private void SetupButtons()
    {
        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);
        playButton.onClick.AddListener(TransitionToMainStage);

        stopRecordingButton.interactable = false;
        playButton.interactable = false;
    }

    private void StartRecording()
    {
        if (poseAICharacter == null)
        {
            Debug.LogError("No PoseAI character assigned!");
            return;
        }

        if (currentAnimationIndex >= animationNames.Length)
        {
            Debug.LogError("All animations have been recorded.");
            return;
        }

        isRecording = true;
        string animationNameWithPlayer = $"P{currentPlayerIndex}_{animationNames[currentAnimationIndex]}";
        Debug.Log($"Starting recording for {animationNameWithPlayer}");
        
        // Start recording the PoseAI model's transforms
        recorder.StartRecording(poseAICharacter.GetComponent<Animator>(), animationNameWithPlayer);
        
        startRecordingButton.interactable = false;
        stopRecordingButton.interactable = true;
    }

    private void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        recorder.StopRecording();

        if (currentAnimationIndex < animationNames.Length)
        {
            string animationNameWithPlayer = $"P{currentPlayerIndex}_{animationNames[currentAnimationIndex]}";
            Debug.Log($"Saving recording for {animationNameWithPlayer}");
            recorder.SaveRecording(animationNameWithPlayer);
            
            // Verify the recording was saved
            AnimationClip savedClip = recorder.GetRecording(animationNameWithPlayer);
            if (savedClip != null)
            {
                Debug.Log($"Successfully saved animation: {animationNameWithPlayer}");
            }
            else
            {
                Debug.LogError($"Failed to save animation: {animationNameWithPlayer}");
            }
            
            currentAnimationIndex++;
        }

        UpdateInstructionText();

        startRecordingButton.interactable = true;
        stopRecordingButton.interactable = false;

        if (currentAnimationIndex >= animationNames.Length)
        {
            if (currentPlayerIndex == 1)
            {
                ResetForPlayer2();
            }
            else
            {
                playButton.interactable = true;
            }
        }
    }

    private void ResetForPlayer2()
    {
        currentPlayerIndex = 2;
        currentAnimationIndex = 0;
        playButton.interactable = false;
        
        if (poseAICharacter != null)
        {
            var animator = poseAICharacter.GetComponent<Animator>();
            animator.runtimeAnimatorController = player2AnimatorController;
            recorder.SetTargetAnimator(animator);
        }
        
        UpdateInstructionText();
    }

    private void UpdateInstructionText()
    {
        if (currentAnimationIndex < animationNames.Length)
        {
            instructionText.text = $"Player {currentPlayerIndex}: Please record the {animationNames[currentAnimationIndex]} animation.";
        }
        else
        {
            if (currentPlayerIndex == 1)
            {
                instructionText.text = "Player 1 recordings complete. Preparing for Player 2...";
            }
            else
            {
                instructionText.text = "All animations have been recorded.";
            }
        }
    }

    private void TransitionToMainStage()
    {
        // Create necessary directories
        string baseDirectory = "Assets/Resources/Animations";
        string p1Directory = $"{baseDirectory}/P1";
        string p2Directory = $"{baseDirectory}/P2";

        // Clean up existing animations
        CleanupExistingAnimations(p1Directory);
        CleanupExistingAnimations(p2Directory);

        // Ensure directories exist
        System.IO.Directory.CreateDirectory(p1Directory);
        System.IO.Directory.CreateDirectory(p2Directory);

        // Save all recorded animations
        foreach (var animationName in animationNames)
        {
            SaveAndConfigureAnimation(1, animationName, p1Directory);
            SaveAndConfigureAnimation(2, animationName, p2Directory);
        }

        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        // Transition to the main stage
        SceneManager.LoadScene("MainStage");
    }

    private void SaveAndConfigureAnimation(int playerIndex, string animationName, string directory)
    {
        string fullAnimName = $"P{playerIndex}_{animationName}";
        AnimationClip clip = recorder.GetRecording(fullAnimName);
        
        if (clip != null)
        {
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            string path = $"{directory}/{animationName}.anim";
            
            // Create a copy of the clip
            AnimationClip clipCopy = new AnimationClip();
            EditorUtility.CopySerialized(clip, clipCopy);
            
            // Save the clip copy
            AssetDatabase.CreateAsset(clipCopy, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            // Configure the saved clip
            AnimationClip savedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (savedClip != null)
            {
                SerializedObject serializedClip = new SerializedObject(savedClip);
                if (serializedClip != null)
                {
                    SerializedProperty settings = serializedClip.FindProperty("m_AnimationClipSettings");
                    if (settings != null)
                    {
                        settings.FindPropertyRelative("m_LoopTime").boolValue = false;
                        settings.FindPropertyRelative("m_KeepOriginalPositionY").boolValue = true;
                        settings.FindPropertyRelative("m_KeepOriginalPositionXZ").boolValue = true;
                        settings.FindPropertyRelative("m_HeightFromFeet").boolValue = false;
                        settings.FindPropertyRelative("m_RootMotionPositionXZ").boolValue = true;
                        serializedClip.ApplyModifiedProperties();
                        AssetDatabase.SaveAssets();
                        Debug.Log($"Successfully configured {fullAnimName}");
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"No recording found for {fullAnimName}");
        }
    }

    private void CleanupExistingAnimations(string directory)
    {
        if (System.IO.Directory.Exists(directory))
        {
            string[] files = System.IO.Directory.GetFiles(directory, "*.anim");
            foreach (string file in files)
            {
                UnityEditor.AssetDatabase.DeleteAsset(file);
            }
        }
    }

    public void SetTargetAnimator(Animator animator)
    {
        poseAICharacter.GetComponent<Animator>().runtimeAnimatorController = animator.runtimeAnimatorController;
    }

    private void AddClipToAnimator(Animator animator, AnimationClip clip, string stateName)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController[stateName] = clip;
        animator.runtimeAnimatorController = overrideController;
    }
}
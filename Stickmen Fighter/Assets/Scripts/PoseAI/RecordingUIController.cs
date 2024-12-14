using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PoseAI;

public class RecordingUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startRecordingButton;
    public Button stopRecordingButton;
    public Button previewAnimationButton;
    public Button playButton;
    public Text instructionText;

    [Header("Recording Settings")]
    public string recordingPath = "Assets/Animations/Recorded/";
    public PoseAICharacterAnimator poseAICharacter;
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
            recorder.SetTargetAnimator(poseAICharacter.GetComponent<Animator>());
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
        previewAnimationButton.onClick.AddListener(PreviewAnimation);
        playButton.onClick.AddListener(TransitionToMainStage);

        stopRecordingButton.interactable = false;
        previewAnimationButton.interactable = false;
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
        previewAnimationButton.interactable = false;
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
        UpdateInstructionText();
    }

    private void PreviewAnimation()
    {
        recorder.PreviewRecording();
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

        // Clean up existing animations in both Recorded and Resources folders
        CleanupExistingAnimations("Assets/Animations/Recorded");
        CleanupExistingAnimations(p1Directory);
        CleanupExistingAnimations(p2Directory);

        // Ensure directories exist
        System.IO.Directory.CreateDirectory(p1Directory);
        System.IO.Directory.CreateDirectory(p2Directory);

        // Save all recorded animations to Resources folder
        foreach (var animationName in animationNames)
        {
            // Save Player 1's animations
            string p1AnimName = $"P1_{animationName}";
            AnimationClip p1Clip = recorder.GetRecording(p1AnimName);
            if (p1Clip != null)
            {
                string p1Path = $"{p1Directory}/{animationName}.anim";
                UnityEditor.AssetDatabase.CreateAsset(p1Clip, p1Path);
                Debug.Log($"Saved {p1AnimName} to Resources");
            }

            // Save Player 2's animations
            string p2AnimName = $"P2_{animationName}";
            AnimationClip p2Clip = recorder.GetRecording(p2AnimName);
            if (p2Clip != null)
            {
                string p2Path = $"{p2Directory}/{animationName}.anim";
                UnityEditor.AssetDatabase.CreateAsset(p2Clip, p2Path);
                Debug.Log($"Saved {p2AnimName} to Resources");
            }
        }

        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();

        // Transition to the main stage
        SceneManager.LoadScene("MainStage");
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
using UnityEngine;
using UnityEngine.UI;

public class RecordingUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startRecordingButton;
    public Button stopRecordingButton;
    public Button saveAnimationButton;
    public Button previewAnimationButton;
    public InputField animationNameInput;

    [Header("Recording Settings")]
    public string recordingPath = "Assets/Animations/Recorded/";
    public Animator targetAnimator; // Reference to the animator you want to record
    private bool isRecording = false;
    private PoseAIAnimationRecorder recorder;

    private void Start()
    {
        recorder = FindObjectOfType<PoseAIAnimationRecorder>();

        if (recorder == null)
        {
            Debug.LogError("PoseAIAnimationRecorder not found in scene!");
            return;
        }

        if (targetAnimator != null)
        {
            recorder.SetTargetAnimator(targetAnimator);
        }
        else
        {
            Debug.LogError("No target animator assigned!");
        }

        SetupButtons();
    }

    private void SetupButtons()
    {
        startRecordingButton.onClick.AddListener(StartRecording);
        stopRecordingButton.onClick.AddListener(StopRecording);
        saveAnimationButton.onClick.AddListener(SaveAnimation);
        previewAnimationButton.onClick.AddListener(PreviewAnimation);

        // Initially disable stop, save, and preview buttons
        stopRecordingButton.interactable = false;
        saveAnimationButton.interactable = false;
        previewAnimationButton.interactable = false;
    }

    private void StartRecording()
    {
        if (targetAnimator == null)
        {
            Debug.LogError("No target animator assigned!");
            return;
        }

        if (string.IsNullOrEmpty(animationNameInput.text))
        {
            Debug.LogWarning("Please enter an animation name before recording");
            return;
        }

        isRecording = true;
        recorder.StartRecording(targetAnimator);

        // Update UI
        startRecordingButton.interactable = false;
        stopRecordingButton.interactable = true;
        saveAnimationButton.interactable = false;
    }

    private void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        recorder.StopRecording();

        // Update UI
        startRecordingButton.interactable = true;
        stopRecordingButton.interactable = false;
        saveAnimationButton.interactable = true;
    }

    private void SaveAnimation()
    {
        string animationName = animationNameInput.text;
        string fullPath = $"{recordingPath}{animationName}.anim";

        recorder.SaveRecording(fullPath);

        // Reset UI
        saveAnimationButton.interactable = false;
        animationNameInput.text = "";
    }

    private void PreviewAnimation()
    {
        recorder.PreviewRecording();
    }

    // Helper method to set the target animator at runtime
    public void SetTargetAnimator(Animator animator)
    {
        targetAnimator = animator;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class PoseAIAnimationRecorder : MonoBehaviour
{
    private AnimationClip currentRecording;
    private Animator targetAnimator;
    private bool isRecording = false;
    private float recordingStartTime;

    // List of properties we want to record
    private readonly string[] propertiesToRecord = new string[]
    {
        // Add the specific properties you want to record from PoseAI
        // These will depend on your PoseAI setup and the bones/transforms you're tracking
        "localPosition.x",
        "localPosition.y",
        "localPosition.z",
        "localRotation.x",
        "localRotation.y",
        "localRotation.z",
        "localRotation.w"
    };

    private Dictionary<string, AnimationCurve> curveDictionary = new Dictionary<string, AnimationCurve>();

    public void StartRecording(Animator animator)
    {
        if (isRecording) return;

        targetAnimator = animator;
        currentRecording = new AnimationClip();
        currentRecording.name = "PoseAI_Recording";

        // Set to legacy for runtime animation creation
        currentRecording.legacy = true;

        recordingStartTime = Time.time;
        isRecording = true;
    }

    private void Update()
    {
        if (!isRecording || targetAnimator == null) return;

        float currentTime = Time.time - recordingStartTime;

        // Record transform data for each bone in the animator's hierarchy
        RecordTransformHierarchy(targetAnimator.transform, currentTime);
    }

    private void RecordTransformHierarchy(Transform transform, float time)
    {
        // Record this transform's data
        string path = GetRelativePath(transform, targetAnimator.transform);

        foreach (string property in propertiesToRecord)
        {
            switch (property)
            {
                case "localPosition.x":
                    AddKeyframe(path + ".localPosition.x", time, transform.localPosition.x);
                    break;
                case "localPosition.y":
                    AddKeyframe(path + ".localPosition.y", time, transform.localPosition.y);
                    break;
                case "localPosition.z":
                    AddKeyframe(path + ".localPosition.z", time, transform.localPosition.z);
                    break;
                case "localRotation.x":
                    AddKeyframe(path + ".localRotation.x", time, transform.localRotation.x);
                    break;
                case "localRotation.y":
                    AddKeyframe(path + ".localRotation.y", time, transform.localRotation.y);
                    break;
                case "localRotation.z":
                    AddKeyframe(path + ".localRotation.z", time, transform.localRotation.z);
                    break;
                case "localRotation.w":
                    AddKeyframe(path + ".localRotation.w", time, transform.localRotation.w);
                    break;
            }
        }

        // Recursively record all children
        foreach (Transform child in transform)
        {
            RecordTransformHierarchy(child, time);
        }
    }

    private string GetRelativePath(Transform target, Transform root)
    {
        if (target == root) return "";

        if (target.parent == root)
            return target.name;

        return GetRelativePath(target.parent, root) + "/" + target.name;
    }

    private void AddKeyframe(string propertyPath, float time, float value)
    {
        Keyframe keyframe = new Keyframe(time, value);
        AnimationCurve curve;

        // Check if the curve already exists in the dictionary
        if (curveDictionary.TryGetValue(propertyPath, out curve))
        {
            curve.AddKey(keyframe);
        }
        else
        {
            // Create a new curve if it doesn't exist
            curve = new AnimationCurve(keyframe);
            curveDictionary[propertyPath] = curve;
        }

        // Set the curve in the animation clip
        currentRecording.SetCurve("", typeof(Transform), propertyPath, curve);
    }

    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        targetAnimator = null;
    }

    public void SaveRecording(string animationName)
    {
        if (currentRecording == null) return;

#if UNITY_EDITOR
        // Define the path where the animation will be saved
        string directory = "Assets/Animations/Recorded";
        string path = $"{directory}/{animationName}.anim";

        // Ensure the directory exists
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        // Save the animation clip as an asset
        UnityEditor.AssetDatabase.CreateAsset(currentRecording, path);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Animation saved as: {path}");
#endif
    }

    public void PreviewRecording()
    {
        if (currentRecording == null || targetAnimator == null) return;

        // Create a temporary animation clip for preview
        AnimationClip previewClip = new AnimationClip();
        foreach (var kvp in curveDictionary)
        {
            previewClip.SetCurve("", typeof(Transform), kvp.Key, kvp.Value);
        }

        // Create a temporary AnimatorOverrideController to apply the clip
        AnimatorOverrideController overrideController = new AnimatorOverrideController(targetAnimator.runtimeAnimatorController);

        // Ensure the state name matches the one in your Animator Controller
        string stateName = "Idle Walk Run Blend"; // Replace with the actual state name
        if (overrideController[stateName] != null)
        {
            overrideController[stateName] = previewClip;
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in Animator Controller.");
            return;
        }

        targetAnimator.runtimeAnimatorController = overrideController;
        targetAnimator.Play(stateName);
    }

    public void AddPunchAnimationToController(string stateName)
    {
        if (currentRecording == null || targetAnimator == null) return;

        // Create a temporary animation clip for the punch
        AnimationClip punchClip = new AnimationClip();
        foreach (var kvp in curveDictionary)
        {
            punchClip.SetCurve("", typeof(Transform), kvp.Key, kvp.Value);
        }

        // Create a temporary AnimatorOverrideController to apply the clip
        AnimatorOverrideController overrideController = new AnimatorOverrideController(targetAnimator.runtimeAnimatorController);

        // Ensure the state name matches the one in your Animator Controller
        if (overrideController[stateName] != null)
        {
            overrideController[stateName] = punchClip;
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in Animator Controller.");
            return;
        }

        targetAnimator.runtimeAnimatorController = overrideController;
        targetAnimator.Play(stateName);
    }

    public void SetTargetAnimator(Animator animator)
    {
        targetAnimator = animator;
    }
}
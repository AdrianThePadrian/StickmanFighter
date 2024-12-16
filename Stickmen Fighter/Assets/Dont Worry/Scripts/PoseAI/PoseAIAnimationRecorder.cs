using UnityEngine;
using System.Collections.Generic;
using System.Linq;  
using PoseAI;
using UnityEditor;

public class PoseAIAnimationRecorder : MonoBehaviour
{
    private AnimationClip currentRecording;
    public Animator targetAnimator;
    private PoseAICharacterAnimator poseAIAnimator;
    private bool isRecording = false;
    private float recordingStartTime;

    // Animation recording settings
    private const float RECORDING_DURATION = 2.0f;
    private const float FRAMES_PER_SECOND = 30.0f;
    private float nextFrameTime = 0f;

    private readonly HumanBodyBones[] bonesToRecord = {
        HumanBodyBones.Hips,
        HumanBodyBones.Spine,
        HumanBodyBones.Chest,
        HumanBodyBones.UpperChest,
        HumanBodyBones.Neck,
        HumanBodyBones.Head,
        HumanBodyBones.LeftShoulder,
        HumanBodyBones.LeftUpperArm,
        HumanBodyBones.LeftLowerArm,
        HumanBodyBones.LeftHand,
        HumanBodyBones.RightShoulder,
        HumanBodyBones.RightUpperArm,
        HumanBodyBones.RightLowerArm,
        HumanBodyBones.RightHand,
        HumanBodyBones.LeftUpperLeg,
        HumanBodyBones.LeftLowerLeg,
        HumanBodyBones.LeftFoot,
        HumanBodyBones.RightUpperLeg,
        HumanBodyBones.RightLowerLeg,
        HumanBodyBones.RightFoot
    };

    private Dictionary<string, AnimationCurve> curveDictionary = new Dictionary<string, AnimationCurve>();
    private Dictionary<string, AnimationClip> recordings = new Dictionary<string, AnimationClip>();

    public void StartRecording(Animator animator, string animationName)
    {
        if (isRecording) return;

        targetAnimator = animator;
        poseAIAnimator = animator.GetComponent<PoseAICharacterAnimator>();
        
        if (poseAIAnimator != null)
        {
            poseAIAnimator.AnimationAlpha = 1.0f;
        }
        
        currentRecording = new AnimationClip();
        currentRecording.name = animationName;
        currentRecording.legacy = false;
        currentRecording.frameRate = FRAMES_PER_SECOND;
        currentRecording.wrapMode = WrapMode.Once;
        
        curveDictionary.Clear();
        recordingStartTime = Time.time;
        nextFrameTime = 0f;
        isRecording = true;
        Debug.Log($"Started recording animation: {animationName}");
    }

    private void Update()
    {
        if (!isRecording || targetAnimator == null) return;

        float currentTime = Time.time - recordingStartTime;
        
        // Stop recording if we've reached the duration
        if (currentTime >= RECORDING_DURATION)
        {
            StopRecording();
            return;
        }

        // Record frame at fixed intervals
        if (currentTime >= nextFrameTime)
        {
            RecordFrame(currentTime);
            nextFrameTime = currentTime + (1f / FRAMES_PER_SECOND);
            Debug.Log($"Recording frame at time: {currentTime:F2}");
        }
    }

    private void RecordFrame(float currentTime)
    {
        if (targetAnimator == null) return;

        Transform hips = targetAnimator.GetBoneTransform(HumanBodyBones.Hips);
        if (hips != null)
        {
            // Record root motion in world space
            Vector3 rootPosition = hips.position;
            AddCurveForProperty("RootT.x", currentTime, rootPosition.x);
            AddCurveForProperty("RootT.y", currentTime, rootPosition.y);
            AddCurveForProperty("RootT.z", currentTime, rootPosition.z);

            Quaternion rootRotation = hips.rotation;
            AddCurveForProperty("RootQ.x", currentTime, rootRotation.x);
            AddCurveForProperty("RootQ.y", currentTime, rootRotation.y);
            AddCurveForProperty("RootQ.z", currentTime, rootRotation.z);
            AddCurveForProperty("RootQ.w", currentTime, rootRotation.w);
        }

        // Record each bone's local position, rotation, and scale
        foreach (HumanBodyBones bone in bonesToRecord)
        {
            Transform boneTransform = targetAnimator.GetBoneTransform(bone);
            if (boneTransform != null)
            {
                string bonePath = AnimationUtility.CalculateTransformPath(boneTransform, targetAnimator.transform);
                
                // Record local position
                Vector3 localPosition = boneTransform.localPosition;
                AddCurveForProperty($"{bonePath}.localPosition.x", currentTime, localPosition.x);
                AddCurveForProperty($"{bonePath}.localPosition.y", currentTime, localPosition.y);
                AddCurveForProperty($"{bonePath}.localPosition.z", currentTime, localPosition.z);

                // Record local rotation as quaternion
                Quaternion localRotation = boneTransform.localRotation;
                AddCurveForProperty($"{bonePath}.localRotation.x", currentTime, localRotation.x);
                AddCurveForProperty($"{bonePath}.localRotation.y", currentTime, localRotation.y);
                AddCurveForProperty($"{bonePath}.localRotation.z", currentTime, localRotation.z);
                AddCurveForProperty($"{bonePath}.localRotation.w", currentTime, localRotation.w);

                // Record local scale
                Vector3 localScale = boneTransform.localScale;
                AddCurveForProperty($"{bonePath}.localScale.x", currentTime, localScale.x);
                AddCurveForProperty($"{bonePath}.localScale.y", currentTime, localScale.y);
                AddCurveForProperty($"{bonePath}.localScale.z", currentTime, localScale.z);
            }
        }
    }

    private void AddCurveForProperty(string propertyPath, float time, float value)
{
    if (!curveDictionary.ContainsKey(propertyPath))
    {
        curveDictionary[propertyPath] = new AnimationCurve();
    }
    
    var keyframe = new Keyframe(time, value);
    curveDictionary[propertyPath].AddKey(keyframe);
}

    public void StopRecording()
{
    if (!isRecording || currentRecording == null) return;
    isRecording = false;

    foreach (var kvp in curveDictionary)
    {
        string propertyPath = kvp.Key;
        AnimationCurve curve = kvp.Value;

        if (propertyPath.StartsWith("RootT") || propertyPath.StartsWith("RootQ"))
        {
            currentRecording.SetCurve("", typeof(Animator), propertyPath, curve);
        }
        else
        {
            // Extract the transform path and property
            string bonePath = propertyPath.Substring(0, propertyPath.LastIndexOf('.'));
            string property = propertyPath.Substring(propertyPath.LastIndexOf('.') + 1);
            currentRecording.SetCurve(bonePath, typeof(Transform), property, curve);
        }
    }

    // Save the recording first
    recordings[currentRecording.name] = currentRecording;

    // Now configure the animation settings
    if (UnityEditor.AssetDatabase.Contains(currentRecording))
    {
        SerializedObject serializedClip = new SerializedObject(currentRecording);
        SerializedProperty settings = serializedClip.FindProperty("m_AnimationClipSettings");
        if (settings != null)
        {
            settings.FindPropertyRelative("m_LoopTime").boolValue = false;
            settings.FindPropertyRelative("m_KeepOriginalPositionY").boolValue = true;
            settings.FindPropertyRelative("m_KeepOriginalPositionXZ").boolValue = true;
            settings.FindPropertyRelative("m_HeightFromFeet").boolValue = false;
            settings.FindPropertyRelative("m_RootMotionPositionXZ").boolValue = true;
            serializedClip.ApplyModifiedProperties();
        }
    }

    Debug.Log($"Successfully stopped recording: {currentRecording.name} with {curveDictionary.Count} curves");
}

    public void SaveRecording(string animationName)
    {
        if (recordings.ContainsKey(animationName))
        {
            Debug.Log($"Recording {animationName} saved in memory with {recordings[animationName].length:F2}s duration");
        }
        else
        {
            Debug.LogWarning($"No recording found for animation: {animationName}");
        }
    }

    public AnimationClip GetRecording(string animationName)
    {
        if (recordings.TryGetValue(animationName, out AnimationClip clip))
        {
            return clip;
        }
        Debug.LogWarning($"Recording for animation '{animationName}' not found.");
        return null;
    }

    public void SetTargetAnimator(Animator animator)
    {
        targetAnimator = animator;
        if (animator != null)
        {
            animator.updateMode = AnimatorUpdateMode.Normal;
        }
    }

    private void RecordTransform(Transform transform, string path)
    {
        // Only record position, rotation, and scale
        string posPath = $"{path}.localPosition";
        string rotPath = $"{path}.localRotation";
        string scalePath = $"{path}.localScale";

        if (!curveDictionary.ContainsKey(posPath))
        {
            curveDictionary[posPath] = new AnimationCurve();
            curveDictionary[rotPath] = new AnimationCurve();
            curveDictionary[scalePath] = new AnimationCurve();
        }

        // Record the current values
        float time = Time.time - recordingStartTime;
        
        Vector3 position = transform.localPosition;
        curveDictionary[posPath].AddKey(time, position.x);
        curveDictionary[posPath].AddKey(time, position.y);
        curveDictionary[posPath].AddKey(time, position.z);

        Quaternion rotation = transform.localRotation;
        curveDictionary[rotPath].AddKey(time, rotation.x);
        curveDictionary[rotPath].AddKey(time, rotation.y);
        curveDictionary[rotPath].AddKey(time, rotation.z);
        curveDictionary[rotPath].AddKey(time, rotation.w);

        Vector3 scale = transform.localScale;
        curveDictionary[scalePath].AddKey(time, scale.x);
        curveDictionary[scalePath].AddKey(time, scale.y);
        curveDictionary[scalePath].AddKey(time, scale.z);
    }
}
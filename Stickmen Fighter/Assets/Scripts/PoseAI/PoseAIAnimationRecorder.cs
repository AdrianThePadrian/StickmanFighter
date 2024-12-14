using UnityEngine;
using System.Collections.Generic;
using System.Linq;  
using PoseAI;

public class PoseAIAnimationRecorder : MonoBehaviour
{
    private AnimationClip currentRecording;
    public Animator targetAnimator;
    private PoseAICharacterAnimator poseAIAnimator;
    private bool isRecording = false;
    private float recordingStartTime;
    private bool shouldRecordFrame = false;

    private readonly string[] propertiesToRecord = new string[]
    {
        "m_LocalPosition.x",
        "m_LocalPosition.y",
        "m_LocalPosition.z",
        "m_LocalRotation.x",
        "m_LocalRotation.y",
        "m_LocalRotation.z",
        "m_LocalRotation.w"
    };

    private Dictionary<string, AnimationCurve> curveDictionary = new Dictionary<string, AnimationCurve>();
    private Dictionary<string, AnimationClip> recordings = new Dictionary<string, AnimationClip>();

    public void StartRecording(Animator animator, string animationName)
    {
        if (isRecording) return;

        targetAnimator = animator;
        poseAIAnimator = animator.GetComponent<PoseAICharacterAnimator>();
        
        currentRecording = new AnimationClip();
        currentRecording.name = animationName;
        curveDictionary.Clear();
        recordingStartTime = Time.time;
        isRecording = true;
        Debug.Log($"Started recording animation: {animationName}");
    }

    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        if (currentRecording != null)
        {
            // Set animation settings
            currentRecording.legacy = false;
            currentRecording.wrapMode = WrapMode.Once;

            // Group curves by path and property
            var groupedCurves = new Dictionary<string, Dictionary<string, AnimationCurve>>();
            
            foreach (var kvp in curveDictionary)
            {
                string[] parts = kvp.Key.Split('.');
                string path = parts[0];
                string property = string.Join(".", parts.Skip(1));
                
                if (!groupedCurves.ContainsKey(path))
                    groupedCurves[path] = new Dictionary<string, AnimationCurve>();
                    
                groupedCurves[path][property] = kvp.Value;
            }

            // Set curves properly grouped by transform
            foreach (var pathGroup in groupedCurves)
            {
                string path = pathGroup.Key;
                var curves = pathGroup.Value;

                // Set position curves
                if (curves.ContainsKey("m_LocalPosition.x") &&
                    curves.ContainsKey("m_LocalPosition.y") &&
                    curves.ContainsKey("m_LocalPosition.z"))
                {
                    currentRecording.SetCurve(path, typeof(Transform), "localPosition.x", curves["m_LocalPosition.x"]);
                    currentRecording.SetCurve(path, typeof(Transform), "localPosition.y", curves["m_LocalPosition.y"]);
                    currentRecording.SetCurve(path, typeof(Transform), "localPosition.z", curves["m_LocalPosition.z"]);
                }

                // Set rotation curves
                if (curves.ContainsKey("m_LocalRotation.x") &&
                    curves.ContainsKey("m_LocalRotation.y") &&
                    curves.ContainsKey("m_LocalRotation.z") &&
                    curves.ContainsKey("m_LocalRotation.w"))
                {
                    currentRecording.SetCurve(path, typeof(Transform), "localRotation.x", curves["m_LocalRotation.x"]);
                    currentRecording.SetCurve(path, typeof(Transform), "localRotation.y", curves["m_LocalRotation.y"]);
                    currentRecording.SetCurve(path, typeof(Transform), "localRotation.z", curves["m_LocalRotation.z"]);
                    currentRecording.SetCurve(path, typeof(Transform), "localRotation.w", curves["m_LocalRotation.w"]);
                }
            }

            recordings[currentRecording.name] = currentRecording;
            Debug.Log($"Stopped recording animation: {currentRecording.name}");
        }
    }

    private void LateUpdate()
    {
        if (shouldRecordFrame)
        {
            float currentTime = Time.time - recordingStartTime;
            RecordTransformHierarchy(targetAnimator.transform, currentTime);
            shouldRecordFrame = false;
        }
    }

    public void OnAnimatorIK()
    {
        if (isRecording && targetAnimator != null)
        {
            shouldRecordFrame = true;
        }
    }

    private void OnEnable()
    {
        // Subscribe to animator IK callback
        if (targetAnimator != null)
        {
            targetAnimator.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        }
    }

    private void RecordTransformHierarchy(Transform transform, float time)
    {
        // Only record humanoid bones
        HumanBodyBones[] bonesToRecord = {
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

        foreach (HumanBodyBones bone in bonesToRecord)
        {
            Transform boneTransform = targetAnimator.GetBoneTransform(bone);
            if (boneTransform != null)
            {
                string path = GetRelativePath(boneTransform, targetAnimator.transform);
                
                // Record position and rotation for each bone
                AddCurveForProperty(path, "m_LocalPosition", boneTransform.localPosition, time);
                AddCurveForProperty(path, "m_LocalRotation", boneTransform.localRotation, time);
            }
        }
    }

    private void AddCurveForProperty(string path, string propertyName, Vector3 value, float time)
    {
        if (propertyName == "m_LocalPosition")
        {
            AddKey($"{path}.localPosition.x", time, value.x);
            AddKey($"{path}.localPosition.y", time, value.y);
            AddKey($"{path}.localPosition.z", time, value.z);
        }
    }

    private void AddCurveForProperty(string path, string propertyName, Quaternion value, float time)
    {
        if (propertyName == "m_LocalRotation")
        {
            AddKey($"{path}.localRotation.x", time, value.x);
            AddKey($"{path}.localRotation.y", time, value.y);
            AddKey($"{path}.localRotation.z", time, value.z);
            AddKey($"{path}.localRotation.w", time, value.w);
        }
    }

    private void AddKey(string propertyPath, float time, float value)
    {
        if (!curveDictionary.ContainsKey(propertyPath))
        {
            curveDictionary[propertyPath] = new AnimationCurve();
        }
        curveDictionary[propertyPath].AddKey(time, value);
    }

    private string GetRelativePath(Transform current, Transform root)
    {
        if (current == root) return "";
        if (current.parent == root) return current.name;
        return GetRelativePath(current.parent, root) + "/" + current.name;
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
    }

    public void SaveRecording(string animationName)
    {
        if (recordings.ContainsKey(animationName))
        {
            Debug.Log($"Recording {animationName} saved in memory");
        }
        else
        {
            Debug.LogError($"No recording found to save for {animationName}");
        }
    }

    public void PreviewRecording()
    {
        if (currentRecording == null || targetAnimator == null)
        {
            Debug.LogWarning("No recording to preview or no target animator set");
            return;
        }

        // Create a temporary AnimatorOverrideController
        AnimatorOverrideController overrideController = new AnimatorOverrideController(targetAnimator.runtimeAnimatorController);
        
        // Get the first state name from the controller
        var currentClipInfo = targetAnimator.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo.Length > 0)
        {
            string stateName = currentClipInfo[0].clip.name;
            overrideController[stateName] = currentRecording;
            targetAnimator.runtimeAnimatorController = overrideController;
            Debug.Log($"Previewing animation on state: {stateName}");
        }
        else
        {
            Debug.LogError("No animation states found in animator controller");
        }
    }
}
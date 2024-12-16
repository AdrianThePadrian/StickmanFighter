using UnityEditor;
using UnityEngine;

public class CreateBasicAnimatorMenu
{
    [MenuItem("Tools/Create Basic Player Animator")]
    private static void CreateBasicAnimator()
    {
        var setup = ScriptableObject.CreateInstance<BasicPlayerAnimatorSetup>();
        setup.CreateAnimatorController();
        Debug.Log("Basic player animator controller created!");
    }
} 
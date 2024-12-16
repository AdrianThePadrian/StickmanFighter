using UnityEngine;

[CreateAssetMenu(fileName = "BasicPlayerAnimator", menuName = "Animation/Basic Player Animator")]
public class BasicPlayerAnimatorSetup : ScriptableObject
{
    // Add references for default animation clips
    public AnimationClip defaultIdleAnimation;
    public AnimationClip defaultMovingAnimation;
    public AnimationClip defaultHighAttackAnimation;
    public AnimationClip defaultLowAttackAnimation;
    public AnimationClip defaultVictoryAnimation;
    public AnimationClip defaultDefeatAnimation;

    public RuntimeAnimatorController CreateAnimatorController()
    {
        // Validate animations
        if (defaultIdleAnimation == null || defaultMovingAnimation == null || 
            defaultHighAttackAnimation == null || defaultLowAttackAnimation == null ||
            defaultVictoryAnimation == null || defaultDefeatAnimation == null)
        {
            Debug.LogError("Missing animation clips! Please assign all animations in the inspector.");
            return null;
        }

        // Create the controller
        var controller = new UnityEditor.Animations.AnimatorController();
        UnityEditor.AssetDatabase.CreateAsset(controller, "Assets/Animation/BasicPlayerController.controller");

        // Add parameters
        controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);
        controller.AddParameter("HighAttack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("LowAttack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Victory", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Defeat", AnimatorControllerParameterType.Trigger);

        // Create and configure the layer
        var rootStateMachine = new UnityEditor.Animations.AnimatorStateMachine();
        UnityEditor.AssetDatabase.AddObjectToAsset(rootStateMachine, controller);
        controller.AddLayer("Base Layer");
        controller.layers[0].stateMachine = rootStateMachine;

        // Create states and assign default motions
        var idleState = rootStateMachine.AddState("Idle");
        idleState.motion = defaultIdleAnimation;
        
        var movingState = rootStateMachine.AddState("Moving");
        movingState.motion = defaultMovingAnimation;
        
        var highAttackState = rootStateMachine.AddState("HighAttack");
        highAttackState.motion = defaultHighAttackAnimation;
        
        var lowAttackState = rootStateMachine.AddState("LowAttack");
        lowAttackState.motion = defaultLowAttackAnimation;
        
        var victoryState = rootStateMachine.AddState("Victory");
        victoryState.motion = defaultVictoryAnimation;
        
        var defeatState = rootStateMachine.AddState("Defeat");
        defeatState.motion = defaultDefeatAnimation;

        // Set Idle as default state
        rootStateMachine.defaultState = idleState;

        // Create basic transitions
        rootStateMachine.AddAnyStateTransition(highAttackState);
        rootStateMachine.AddAnyStateTransition(lowAttackState);
        rootStateMachine.AddAnyStateTransition(victoryState);
        rootStateMachine.AddAnyStateTransition(defeatState);

        // Add Idle <-> Moving transitions
        var idleToMoving = idleState.AddTransition(movingState);
        idleToMoving.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "IsMoving");
        
        var movingToIdle = movingState.AddTransition(idleState);
        movingToIdle.AddCondition(UnityEditor.Animations.AnimatorConditionMode.IfNot, 0, "IsMoving");

        UnityEditor.AssetDatabase.SaveAssets();
        return controller;
    }
} 
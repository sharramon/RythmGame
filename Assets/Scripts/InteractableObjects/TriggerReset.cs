using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TriggerReset : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var parameter in animator.parameters.Where(parameter => parameter.type == AnimatorControllerParameterType.Trigger))
        {
            animator.ResetTrigger(parameter.name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArmController : MonoBehaviour
{
   public ConfigurableJoint LeftArmJoint,RightArmJoint;
    public Vector3 LeftArmTargetRotation;
    public Vector3 RightArmTargetRotation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Move left arm to target rotation
            SetJointTargetRotation(LeftArmJoint, LeftArmTargetRotation);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Move right arm to target rotation
            SetJointTargetRotation(RightArmJoint, RightArmTargetRotation);
        }
    }

    private void SetJointTargetRotation(ConfigurableJoint joint, Vector3 targetRotation)
    {
        // Convert the target rotation from Euler angles to a Quaternion
        Quaternion targetQuat = Quaternion.Euler(targetRotation);

        // Set the target rotation of the ConfigurableJoint
        joint.targetRotation = targetQuat;
    }
}

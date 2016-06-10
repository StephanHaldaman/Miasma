using UnityEngine;
using System.Collections;

public class AnimationJoint : MonoBehaviour
{
    public string pivotAngle;
    public float idleAngle;
    public float[] jointAngles;

    public int _jointIndex = 0;

    public void UpdateJointPosition(float pivotSpeed) 
    {
        if (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.z, jointAngles[_jointIndex])) > 1)
        {
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                Mathf.MoveTowards(transform.localEulerAngles.z, jointAngles[_jointIndex], pivotSpeed * Time.deltaTime)
                );
        }
        else
        {
            // Change index
            _jointIndex++;
            if (_jointIndex == jointAngles.Length)
            {
                _jointIndex = 0;
            }
        }
    }
}

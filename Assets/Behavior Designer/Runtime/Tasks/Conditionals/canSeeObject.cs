using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Returns success as soon as the event specified by eventName has been received.")]
    [TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
    public class canSeeObject : Conditional
    {
        [Tooltip("The name of the event to receive")]
        public SharedGameObject goalObject; 
        [Tooltip("Optionally store the first sent argument")]
        [SharedRequired]
        public SharedFloat viewRadius;
        [Tooltip("Optionally store the second sent argument")]
        [SharedRequired]
        public SharedFloat viewAngle;


        public override void OnStart()
        {
            // Let the behavior tree know that we are interested in receiving the event specified
            
        }

        public override TaskStatus OnUpdate()
        {
            if(viewRadius.Value > Vector3.Distance(gameObject.transform.position, goalObject.Value.transform.position))
            {

                /*Debug.Log("dis=" + Vector3.Distance(gameObject.transform.position, goalObject.Value.transform.position) 
                    + "£¬angle="+ Vector3.Angle(gameObject.transform.forward, goalObject.Value.transform.position - gameObject.transform.position));*/
                if (viewAngle.Value > Vector3.Angle(gameObject.transform.forward, goalObject.Value.transform.position- gameObject.transform.position))
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
        public override void OnEnd()
        {

        }


        public override void OnBehaviorComplete()
        {
            
        }

        public override void OnReset()
        {
            // Reset the properties back to their original values
        }
    }
}
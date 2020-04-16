using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform cameraCenter;
    private Transform forwardObject;
    private float verticalViewAngle = 30;
    private float horizontalViewAngle = 180;
    private float viewAngleSensitivity = 50;//视角灵敏度,最快每秒多少度
    void Start()
    {
        cameraCenter = transform.Find("cameraCenter");
        forwardObject = transform.Find("forwardObject");
    }

    void Update()
    {
        float v = Input.GetAxis("cameraVertical");
        float h = Input.GetAxis("cameraHorizontal");
        float see_v = Input.GetAxis("seeVertical");
        float see_h = Input.GetAxis("seeHorizontal");

        float rotationX = forwardObject.localRotation.eulerAngles.x + v * viewAngleSensitivity * Time.deltaTime;
        if (rotationX > 180) rotationX = rotationX - 360;
        forwardObject.rotation = Quaternion.Euler(
            Mathf.Min(Mathf.Max(rotationX, -verticalViewAngle), verticalViewAngle)
            , forwardObject.localRotation.eulerAngles.y + h * viewAngleSensitivity *Time.deltaTime, 0);

        if(see_v != 0 || see_h != 0)
        {
            Vector3 seeRotate = new Vector3(see_v * verticalViewAngle, see_h * horizontalViewAngle, 0) +forwardObject.localRotation.eulerAngles;
            if (seeRotate.x > 180) seeRotate.x = 360 - seeRotate.x;
            seeRotate.x = Mathf.Max(-verticalViewAngle, Mathf.Min(verticalViewAngle, seeRotate.x));
            cameraCenter.localRotation = Quaternion.Euler(seeRotate);
        }
        else cameraCenter.forward = forwardObject.forward;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamPlayer : MonoBehaviour
{
    public Transform objectTofollow; // 따라가야할 오브젝트 정보

    public float followspeed = 10f;     // 따라가는 스피드
    public float sensitivity = 100f;    // 마우스 감도
    public float clampAngle = 35f;      // 카메라 앵글 각도 70도


    private float rotX;
    private float rotY;
    public float RotX { get { return rotX; } set { rotX = value; } }
    public float RotY { get { return rotY; } set { rotY = value; } }


    public float minDistance;
    public float maxDistance;
    [SerializeField] float smoothness;
    [SerializeField] float finalDistance;
    [SerializeField] Vector3 finalDir;
    [SerializeField] Vector3 dirNormalized;
    [SerializeField] Transform realCamera;



    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;
    }


    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButton(1))
        {
            



        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;


        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        
        }


    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followspeed * Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.position, finalDir, out hit))
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        else
            finalDistance = maxDistance;

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }

}

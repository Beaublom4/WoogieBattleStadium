using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera cam;
    public RectTransform gameView;

    public float moveSpeed;
    private float calcMoveSpeed, calcRotSpeed;
    public CheckPoints[] checkPoints;
    public Transform yourCheckpoint, otherCheckpoint;

    private Transform target;
    private Vector3 lastPos;
    private Quaternion lastRot;
    private int currentTarget = 0, currentCheckPoint = 0;
    private bool locked = false;

    private void Start()
    {
        cam.targetTexture = new RenderTexture((int)gameView.rect.width * 3, (int)gameView.rect.height * 3, 24);
        gameView.GetComponent<RawImage>().texture = cam.targetTexture;

        ResetTarget();
    }
    private void ResetTarget()
    {
        target = checkPoints[0].targets[0];
        cam.transform.SetPositionAndRotation(target.position, transform.rotation);

        calcMoveSpeed = (target.position - cam.transform.position).magnitude * moveSpeed;
        calcRotSpeed = (target.rotation.eulerAngles - cam.transform.rotation.eulerAngles).magnitude * moveSpeed;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (locked)
            return;

        bool targetHasChanged = false;
        //Move camera to next target

        cam.transform.position = Vector3.MoveTowards(cam.transform.position, target.position, calcMoveSpeed * Time.deltaTime);
        cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, target.rotation, calcRotSpeed * Time.deltaTime);
        if (Vector3.Distance(cam.transform.position, target.position) < .01f && Vector3.Distance(cam.transform.rotation.eulerAngles, target.rotation.eulerAngles) < .01f)
        {
            currentTarget++;
            //Check if current target bigger than targets in current checkpoint
            if(currentTarget >= checkPoints[currentCheckPoint].targets.Length)
            {
                targetHasChanged = true;
                currentTarget = 0;
                currentCheckPoint++;
                //Check if current checkpoint is out of range in check points
                if (currentCheckPoint >= checkPoints.Length)
                {
                    currentCheckPoint = 0;
                }
            }
            //Set next target
            target = checkPoints[currentCheckPoint].targets[currentTarget];
            if (targetHasChanged)
            {
                cam.transform.SetPositionAndRotation(target.position, target.rotation);
            }
            calcMoveSpeed = (target.position - cam.transform.position).magnitude * moveSpeed;
            calcRotSpeed = (target.rotation.eulerAngles - cam.transform.rotation.eulerAngles).magnitude * moveSpeed;
        }
    }
    public void PerspectiveYou()
    {
        LockCam();
        StartCoroutine(PerspectiveMove(yourCheckpoint.position, yourCheckpoint.rotation));
    }
    public void PerspectiveOther()
    {
        LockCam();
        StartCoroutine(PerspectiveMove(otherCheckpoint.position, otherCheckpoint.rotation));
    }
    public IEnumerator PerspectiveMove(Vector3 pos, Quaternion rot)
    {
        calcMoveSpeed = (pos - cam.transform.position).magnitude * 50;
        calcRotSpeed = (rot.eulerAngles - cam.transform.rotation.eulerAngles).magnitude * 50;

        while (Vector3.Distance(cam.transform.position, pos) > .01f || Vector3.Distance(cam.transform.rotation.eulerAngles, rot.eulerAngles) > .01f)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, pos, calcMoveSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, rot, calcRotSpeed * Time.deltaTime);
            yield return null;
        }

        lastPos = checkPoints[0].targets[0].position;
        lastRot = checkPoints[0].targets[0].rotation;
        target = checkPoints[0].targets[1];
    }
    public void LockCam()
    {
        if (!locked)
        {
            lastPos = cam.transform.position;
            lastRot = cam.transform.rotation;
        }
        locked = true;
    }
    public void UnlockCam()
    {
        cam.transform.SetPositionAndRotation(lastPos, lastRot);
        calcMoveSpeed = (target.position - cam.transform.position).magnitude * moveSpeed;
        calcRotSpeed = (target.rotation.eulerAngles - cam.transform.rotation.eulerAngles).magnitude * moveSpeed;
        locked = false;
    }
}
[System.Serializable]
public struct CheckPoints
{
    public Transform[] targets;
}

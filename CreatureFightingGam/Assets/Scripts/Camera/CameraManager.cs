using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Camera cam;
    public RectTransform gameView;

    public float moveSpeed;
    public CheckPoints[] checkPoints;
    private Transform target;
    private int currentTarget = 0, currentCheckPoint = 0;

    private void Start()
    {
        cam.targetTexture = new RenderTexture((int)gameView.rect.width * 3, (int)gameView.rect.height * 3, 24);
        gameView.GetComponent<RawImage>().texture = cam.targetTexture;

        target = checkPoints[0].targets[0];
        cam.transform.position = target.position;
        cam.transform.rotation = transform.rotation;
    }
    private void Update()
    {
        bool targetHasChanged = false;
        //Move camera to next target
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, target.position, moveSpeed * Time.deltaTime);
        //cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, target.rotation, moveSpeed * Time.deltaTime);
        if(cam.transform.position == target.position)
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
                cam.transform.position = target.position;
                cam.transform.rotation = target.rotation;
            }
        }
    }
}
[System.Serializable]
public struct CheckPoints
{
    public Transform[] targets;
}

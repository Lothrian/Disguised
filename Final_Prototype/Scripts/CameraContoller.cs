using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraContoller : MonoBehaviour
{
    // This camera controller allows the camera to rotate around the player object and stick to it

    [Header("Settings")]
    public bool lockCursor;
    public float mouseSensitiviy = 10;
    public Transform target; // The player object
    public float distFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 8f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    [Header("Collision Vars")]

    [Header("Transparancy")]
    public bool changeTransparency = true;
    public MeshRenderer targetRenderer;

    [Header("Speeds")]
    public float moveSpeed = 3;
    public float returnSpeed = 9;
    public float wallPush = 0.7f;

    [Header("Distances")]
    public float closestDistanceToPlayer = 2;
    public float evenCloserDistanceToPlayer = 1;

    [Header("Mask")]
    public LayerMask collisionMask;

    private bool pitchLock = false;

    private void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        //Moves camera with player (target) and checks for collision
        CollisionCheck(target.position - transform.forward * distFromTarget);
        WallCheck();

        //Rotates camera with mouse, only look at yaw if pitch is locked
        if (!pitchLock) {
            yaw += Input.GetAxis("Mouse X") * mouseSensitiviy;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitiviy;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);
        }
        else {
            yaw += Input.GetAxis("Mouse X") * mouseSensitiviy;
            pitch = pitchMinMax.y;
            currentRotation = Vector3.Lerp(currentRotation, new Vector3(pitch, yaw), rotationSmoothTime * Time.deltaTime);
        }

        transform.eulerAngles = currentRotation;

        Vector3 e = transform.eulerAngles;
        e.x = 0;

        target.eulerAngles = e;
    }

    private void WallCheck() {
        Ray ray = new Ray(target.position, -target.forward);
        RaycastHit hit;

        //If it detects a wall 
        if(Physics.SphereCast(ray, 0.2f, out hit, 0.7f, collisionMask)) {
            pitchLock = true;
        }
        else {
            pitchLock = false;
        }
    }

    private void CollisionCheck(Vector3 retPoint) {
        RaycastHit hit;

        //If camera hits object/wall, zoom in
        if(Physics.Linecast(target.position, retPoint, out hit, collisionMask)) {
            Vector3 norm = hit.normal * wallPush;
            Vector3 p = hit.point + norm;

            TransparencyCheck();

            //If future predicted position is lower than our lowest possible distance, do nothing
            if (Vector3.Distance(Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime), target.position) <= evenCloserDistanceToPlayer) {

            }
            else {
                transform.position = Vector3.Lerp(transform.position, p, moveSpeed * Time.deltaTime);

            }
            return;
        }

        //Not near a wall, reset camera and transparency
        FullTransparency();

        transform.position = Vector3.Lerp(transform.position, retPoint, returnSpeed * Time.deltaTime);
        pitchLock = false;
    }

    private void TransparencyCheck() {
        if (changeTransparency) {
            if(Vector3.Distance(transform.position, target.position) <= closestDistanceToPlayer) {
                Color tempCol = targetRenderer.sharedMaterial.color;
                tempCol.a = Mathf.Lerp(tempCol.a, 0.2f, moveSpeed * Time.deltaTime);

                targetRenderer.sharedMaterial.color = tempCol;
                //If target has more than one material, loop through targetRenderer.sharedMaterials

                
            }
            else {
                if(targetRenderer.sharedMaterial.color.a <= 0.99f) {
                    Color tempCol = targetRenderer.sharedMaterial.color;
                    tempCol.a = Mathf.Lerp(tempCol.a, 1, moveSpeed * Time.deltaTime);

                    targetRenderer.sharedMaterial.color = tempCol;
                }
            }
        }
    }

    private void FullTransparency() {
        if (changeTransparency) {
            if (targetRenderer.sharedMaterial.color.a <= 0.99f) {
                Color tempCol = targetRenderer.sharedMaterial.color;
                tempCol.a = Mathf.Lerp(tempCol.a, 1, moveSpeed * Time.deltaTime);

                targetRenderer.sharedMaterial.color = tempCol;
            }
        }
    }
}

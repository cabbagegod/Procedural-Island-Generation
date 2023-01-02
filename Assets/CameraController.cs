using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Values")]
    public float normalMoveSpeed;
    public float fastMoveSpeed;
    public float moveTime;
    public float rotationAmount;
    public float zoomAmount;

    [Header("References")]
    public Transform cameraTransform;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 newZoom;
    private float currentMoveSpeed;

    private void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        HandleMovement();

    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = fastMoveSpeed;
        } else
        {
            currentMoveSpeed = normalMoveSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * currentMoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition += (transform.forward * -currentMoveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition += (transform.right * -currentMoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += (transform.right * currentMoveSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            newZoom += new Vector3(0, -zoomAmount, zoomAmount);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            newZoom -= new Vector3(0, -zoomAmount, zoomAmount);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * moveTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * moveTime);
    }
}

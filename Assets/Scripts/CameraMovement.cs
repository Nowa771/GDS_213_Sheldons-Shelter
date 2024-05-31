using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 20f;  // Speed of the camera panning
    public float zoomSpeed = 5f;  // Speed of the camera zooming
    public float minZoom = 5f;    // Minimum zoom level
    public float maxZoom = 20f;   // Maximum zoom level

    public Vector2 panLimitMin;  // Minimum pan limits (x, y)
    public Vector2 panLimitMax;  // Maximum pan limits (x, y)

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        cam.orthographic = true;  // Ensure the camera is orthographic
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        // Clamp the position to the pan limits
        pos.x = Mathf.Clamp(pos.x, panLimitMin.x, panLimitMax.x);
        pos.y = Mathf.Clamp(pos.y, panLimitMin.y, panLimitMax.y);

        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}

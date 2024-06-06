using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    // Define the maximum bounds within which the camera can move
    public float fixedMinX = -10f, fixedMaxX = 10f, fixedMinY = -10f, fixedMaxY = 10f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        Vector3 position = transform.position;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            position.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            position.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += panSpeed * Time.deltaTime;
        }

        // Calculate effective bounds based on the current zoom level
        float zoomFactor = (cam.orthographicSize - minZoom) / (maxZoom - minZoom);

        float minX = Mathf.Lerp(fixedMinX, fixedMinX - cam.aspect * cam.orthographicSize, 1 - zoomFactor);
        float maxX = Mathf.Lerp(fixedMaxX, fixedMaxX + cam.aspect * cam.orthographicSize, 1 - zoomFactor);
        float minY = Mathf.Lerp(fixedMinY, fixedMinY - cam.orthographicSize, 1 - zoomFactor);
        float maxY = Mathf.Lerp(fixedMaxY, fixedMaxY + cam.orthographicSize, 0 - zoomFactor);

        // Clamp the camera position to the effective bounds
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}

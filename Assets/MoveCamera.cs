using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{
    private const float CameraSpeed = 15f;
    [SerializeField] private Camera _camera;

    private void Reset()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) transform.position = new Vector3(0, 0, transform.position.z);

        var mousePos = Input.mousePosition;
        if (mousePos.x < 50) transform.Translate(-Time.deltaTime * CameraSpeed, 0, 0);
        if (mousePos.y < 50) transform.Translate(0, -Time.deltaTime * CameraSpeed, 0);

        if (mousePos.x >= Screen.width - 50) transform.Translate(Time.deltaTime * CameraSpeed, 0, 0);
        if (mousePos.y >= Screen.height - 50) transform.Translate(0, Time.deltaTime * CameraSpeed, 0);


        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);

        _camera.orthographicSize = Mathf.Max(
            5,
            _camera.orthographicSize + Input.mouseScrollDelta.y
        );
    }
}

using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) transform.position = new Vector3(0, transform.position.y, 0);

        var mousePos = Input.mousePosition;
        if (mousePos.x < 50) transform.Translate(-Time.deltaTime * 5, 0, 0);
        if (mousePos.y < 50) transform.Translate(0, -Time.deltaTime * 5, 0);

        if (mousePos.x >= Screen.width - 50) transform.Translate(Time.deltaTime * 5, 0, 0);
        if (mousePos.y >= Screen.height - 50) transform.Translate(0, Time.deltaTime * 5, 0);
    }
}

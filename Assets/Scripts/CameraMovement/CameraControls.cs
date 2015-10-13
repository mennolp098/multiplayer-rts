using UnityEngine;

public class CameraControls : MonoBehaviour {

    public float movementSpeed;
    public float scrollSpeed;

    private void Update()
    {
        Vector3 vel = Vector3.zero;
        vel.x = Input.GetAxis("Horizontal") * (Time.deltaTime * movementSpeed);
        vel.z = Input.GetAxis("Vertical") * (Time.deltaTime * movementSpeed);
        float up = Input.GetAxis("Mouse ScrollWheel") * (Time.deltaTime * scrollSpeed);

        transform.parent.Translate(vel);
        transform.parent.Translate(up * transform.parent.up);        
    }

}

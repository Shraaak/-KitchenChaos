using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ro : MonoBehaviour
{
    public float speed = 5f;
    public Transform target;
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = Input.GetAxis("Mouse Y");

            Vector3 angles = target.eulerAngles;
            angles.x -= mouse_y;
            target.eulerAngles = angles;
        }
    }
}

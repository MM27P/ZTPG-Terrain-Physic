using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float speed = 1;
    public float sensitivity = 5;
    public float jumpPower = 1;
    bool isFlag = false;
    int countMax = 20;
    int currentCount = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(speed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Move(-speed, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(0, -speed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(0, speed);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Jump(jumpPower);
        }

        if(isFlag)
        {
            currentCount++;
        }

        if( currentCount>countMax)
        {
            isFlag = false;
            currentCount = 0;
        }
        RotateCamera();

        if(transform.position.y <-10)
        {
            transform.position = new Vector3(transform.position.z, 50, transform.position.z);
        }
    }

    void Move(float z, float x)
    {
        rigidbody.MovePosition(transform.position + transform.TransformVector(new Vector3(x, 0, z)));
    }

    void Jump(float force)
    {
        if (rigidbody.velocity.y == 0)
            rigidbody.AddForce(Vector3.up * force);
    }

    void RotateCamera()
    {

        float axisX = Input.GetAxis("Mouse X") * sensitivity;
        float axisY = -Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(0, Time.deltaTime * sensitivity * axisX, 0, Space.World);
        Camera.main.transform.Rotate(Time.deltaTime * sensitivity * axisY, 0, 0, Space.Self);

        if (Input.GetMouseButtonDown(0))
            Cursor.lockState = CursorLockMode.Locked;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;
    public bool rotateX;
    public bool rotateY;
    public bool rotateZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float timePass = Time.deltaTime;
        Transform transform = GetComponent<Transform>();

        Quaternion newRotate = transform.rotation;


        if(rotateX)
        {
            transform .Rotate(speed* timePass,0,0,Space.Self);
        }
        if (rotateY)
        {
            transform.Rotate(0, speed * timePass, 0, Space.Self);
        }
        if (rotateZ)
        {
            transform.Rotate(0, 0, speed * timePass, Space.Self);
        }
    }
}

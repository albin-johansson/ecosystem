using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Update is called once per frame
    private float x;
    private float y;
    private Vector3 rotateValue;
    private float delta = 0.001f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Rotate();
            Translate();
        }
        
        
    }

    private void Translate()
    {
        // For efficiency reasons get transform once
        var currentTransform = transform;
        if (Input.GetKey(KeyCode.W))
        {
            currentTransform.position += speed * delta * currentTransform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentTransform.position -= speed * delta * currentTransform.forward;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            currentTransform.position += speed * delta * currentTransform.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            currentTransform.position -= speed * delta * currentTransform.right;
        }
    }
    
    private void Rotate()
    {
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        Debug.Log(x + ":" + y);
        rotateValue = new Vector3(x, y * -1, 0);
        // For efficiency reasons get transform once
        var transform1 = transform;
        transform1.eulerAngles = transform1.eulerAngles - rotateValue;
    }
}

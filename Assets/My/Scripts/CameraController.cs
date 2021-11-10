using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private  GameObject target;
    private float x1;
    private void Start()
    {
        target = GameObject.Find("Player");

       
        
    }
    private void LateUpdate()
    {
        x1 = Mathf.Clamp(target.transform.position.x, -1.2f, 32f);
    

        transform.position = Vector3.Lerp(transform.position, new Vector3(x1, 0.4f, -10f), 0.05f);

    }

   
}

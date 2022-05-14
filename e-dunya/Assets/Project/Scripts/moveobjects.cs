using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveobjects : MonoBehaviour
{       

    
    public float x, y, z;

    Transform trans;

    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = new Vector3(x, y, z);
        transform.Translate(movement);


    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private Camera worldCam;
    private Vector3 mouseLoc;
    private GameObject Projectile; 
    void Start()
    {
        worldCam = GameObject.FindObjectOfType<Camera>(); // find the Camera 

        
    }

    // Update is called once per frame
    void Update()
    {
        mouseLoc = worldCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mouseLoc - transform.position;  
        float rotation2 = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotation2);
     

        if (Input.GetKey(KeyCode.Space))
        {
            fireSomethin();
        }
    }

    private void fireSomethin()
    {

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteMe_door : MonoBehaviour
{
    

    void Awake() 
    {
        
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            var anim = GetComponent<Animation>().Play();
        }  
    }
}

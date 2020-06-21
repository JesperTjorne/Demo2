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
            // GetComponent<Animation>().animatePhysics = false;
            var animation = GetComponent<Animation>();

            animation["Door_Open"].speed = 1;
            animation["Door_Open"].time = 0;

            animation.Play("Door_Open");
            // GetComponent<Animation>().Play("Door_Open");
            // GetComponent<Animator>().SetTrigger("Open");
        }  

        if (Input.GetKeyDown(KeyCode.C))
        {
            // GetComponent<Animation>().Play("Door_Close");
            var animation = GetComponent<Animation>();

            animation["Door_Open"].speed = -1;
            animation["Door_Open"].time = animation["Door_Open"].length;

            animation.Play("Door_Open");
            // GetComponent<Animator>().SetTrigger("Close");
        }
    }
}

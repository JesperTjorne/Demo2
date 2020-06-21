using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteMe_door : MonoBehaviour
{
    

    void Awake() 
    {
        CloseDoor();
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            OpenDoor();
        }  

        if (Input.GetKeyDown(KeyCode.C))
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        var animation = GetComponent<Animation>();

        animation["Door_Open"].speed = 1;
        animation["Door_Open"].time = 0;

        animation.Play("Door_Open");
    }

    private void CloseDoor() 
    {
        var animation = GetComponent<Animation>();

        animation["Door_Open"].speed = -1;
        animation["Door_Open"].time = animation["Door_Open"].length;

        animation.Play("Door_Open");
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (col.tag == "Bot")
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider col) 
    {
        if (col.tag == "Bot")
        {
            CloseDoor();
        }
        
    }
}

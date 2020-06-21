using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private bool m_isOpen;

    public bool m_canClose { get; private set; }

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
        m_isOpen = true;
        var animation = GetComponent<Animation>();

        animation["Door_Open"].speed = 1;
        animation["Door_Open"].time = 0;

        animation.Play("Door_Open");
    }

    private void CloseDoor() 
    {
        m_isOpen = false;
        var animation = GetComponent<Animation>();

        animation["Door_Open"].speed = -1;
        animation["Door_Open"].time = animation["Door_Open"].length;

        animation.Play("Door_Open");
    }

    private void OnTriggerEnter(Collider col) 
    {
        if (col.tag == "Bot")
        {
            if (!m_isOpen)
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Bot")
        {
            m_canClose = false;

        }
    }

    private void OnTriggerExit(Collider col) 
    {
        if (col.tag == "Bot")
        {
            if (m_canClose && m_isOpen)
            {
                CloseDoor();
            }
        }
    }

    IEnumerator Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        m_canClose = true;
    }
}

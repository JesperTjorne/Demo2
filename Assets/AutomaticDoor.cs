using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{

    public GameObject DoorLeft;
    public GameObject DoorRight;

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OpenDoors(DoorLeft.transform);
            OpenDoors(DoorRight.transform);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            OpenDoors(DoorLeft.transform);
            OpenDoors(DoorRight.transform);
        }
    }

    private void OpenDoors(Transform door)
    {
        
                
    }
}

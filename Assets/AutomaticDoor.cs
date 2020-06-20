using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{

    public GameObject DoorLeft;
    public GameObject DoorRight;

    private float m_moveDistance = 2.45f;
    
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
        
        door.transform.position = new Vector3(door.transform.position.x + m_moveDistance, door.transform.position.y, door.transform.position.x);
        
                
    }
}

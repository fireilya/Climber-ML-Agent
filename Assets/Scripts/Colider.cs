using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colider : MonoBehaviour
{

    [SerializeField]
    Climber master;
    private void OnTriggerStay(Collider other)
    {
        if (tag == "FloorColider") master.isFloorColided = true;
        if (tag == "WallColider") master.isWallColided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (tag == "FloorColider") master.isFloorColided = false;
        if (tag == "WallColider") master.isWallColided = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] 
    private Wall wall;

    [SerializeField]
    private float downSpeed = 1;

    private bool isWayBlocked = false;

    public void LiftWall()
    {
        wall.BlockTheWay();
        StartCoroutine(PressButton());
    }
    // Start is called before the first frame update
    IEnumerator PressButton()
    {
        while (transform.localPosition.y>0)
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x, transform.localPosition.y - downSpeed*Time.deltaTime, transform.localPosition.z);
            yield return null;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

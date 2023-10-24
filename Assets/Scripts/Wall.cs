using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float requiredY = 1;
    [SerializeField]
    private float outSpeed = 1;
    
    private IEnumerator BlockTheWayCoroutine()
    {
        while (transform.localPosition.y < requiredY)
        {
            transform.localPosition = new Vector3(
                transform.localPosition.x, transform.localPosition.y + outSpeed * Time.deltaTime, transform.localPosition.z);
            yield return null;
        }
        
    }

    public void StartNewEpisode()
    {
        StopCoroutine(BlockTheWayCoroutine());
    }

    public void BlockTheWay()
    {
        StartCoroutine(BlockTheWayCoroutine());
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}

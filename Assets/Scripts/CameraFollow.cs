using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        if(pos.x > 1.1f) { pos.x = 1.1f; }
        if(pos.x < -1.1f) { pos.x = -1.1f; }
        if(pos.y > 5) { pos.y = 5; }
        if(pos.y < -5) { pos.y = -5; }
        transform.position = pos;
    }
}

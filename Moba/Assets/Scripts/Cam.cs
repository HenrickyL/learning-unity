using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform player;
    private Vector3 cameraOffSet;
    [Range(0.1f, 1.0f)]
    public float smoothness = 0.5f;
    // Start is called before the first frame update
    void Start()
    {cameraOffSet = transform.position - player.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = player.position +cameraOffSet;
        transform.position = Vector3.Slerp(transform.position, newPos,smoothness);
    }
}

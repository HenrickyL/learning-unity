using System;
using UnityEngine;

public class SunController : MonoBehaviour
{
    public float rotSpeed = 1;
    private float desiredRot;
    public float damping = 10;
    float timer = 0f;
    Quaternion initialRotation;
    private void OnEnable() {
         desiredRot = transform.eulerAngles.x;
         initialRotation = transform.rotation;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        desiredRot -= rotSpeed * Time.deltaTime;
        var desiredRotQ = Quaternion.Euler(desiredRot, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * damping);
        if(transform.rotation.x < 0){
            Debug.Log(transform.rotation.x);
            transform.rotation = initialRotation;
            desiredRot = transform.eulerAngles.x;
        }
    }
}

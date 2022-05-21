using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update

    const float  degreesPerHour = -30f;
    const float  degreesPerMinute = -6f;
    const float  degreesPersecond = -6f;
[SerializeField] public Transform hoursTrasnform, minuteTransform, secondTransform;


    private void Awake() {
        DateTime time = DateTime.Now;

        hoursTrasnform.localRotation = Quaternion.Euler(0f,0f,time.Hour*degreesPerHour);
        minuteTransform.localRotation = Quaternion.Euler(0f,0f,time.Minute*degreesPerMinute);
        secondTransform.localRotation = Quaternion.Euler(0f,0f,time.Second*degreesPersecond);

    }

    void Update () {
		DateTime time = DateTime.Now;
		hoursTrasnform.localRotation =
			Quaternion.Euler(0f, 0f, degreesPerHour * time.Hour);
		minuteTransform.localRotation =
			Quaternion.Euler(0f, 0f, degreesPerMinute * time.Minute);
		secondTransform.localRotation =
			Quaternion.Euler(0f, 0f, degreesPersecond * time.Second);
	}
}

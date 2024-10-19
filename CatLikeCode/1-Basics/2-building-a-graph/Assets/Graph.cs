using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    [SerializeField]GameObject point;
    [SerializeField,Range(10,100)]int resolution=10;

    GameObject[] points;

    // Start is called before the first frame update
    private void Awake() {
		float step = 2f / resolution;
        Vector3 scale = Vector3.one*step;
        Vector3 position;
        position.y = 0f;
        position.z = 0f;
        points = new GameObject[resolution];

        for (int i = 0; i < resolution; i++)
        {
            GameObject p =  Instantiate(point);
            p.transform.SetParent(transform,false);
            position.x = ((i+0.5f)*step - 1f);
            position.y = position.x * position.x*position.x + 2*position.x;
            p.transform.localPosition = position;
            p.transform.localScale =scale;
            points[i] = p;
        }
    }

    private void Update() {
        for (int i = 0; i < points.Length; i++)
        {
            GameObject p = points[i];
            Vector3 pos = p.transform.localPosition;
            pos.y = Mathf.Sin(Mathf.PI * (pos.x+Time.time));
            p.transform.localPosition = pos;
         }
    }
}

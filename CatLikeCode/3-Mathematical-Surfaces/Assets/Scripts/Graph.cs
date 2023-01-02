using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
   [SerializeField]
   Transform pointPrefab;
   [SerializeField,Range(10, 100)]
   int resolution = 10;
   List<Transform> points = new List<Transform>();

   void Awake(){
    float step = 2f/resolution;
    var scale = Vector3.one*step;
    Vector3 position = Vector3.zero;
    for(var i=0; i<resolution; i++){
        Transform point = Instantiate(pointPrefab);
        point.SetParent(transform, false);
        position.x = (i+0.5f)*step-1f;
        position.y = position.x * position.x;
        point.localPosition = position;
        point.localScale = scale;
        points.Add(point);
    }
   }
}

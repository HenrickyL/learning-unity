using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public Metrics(int corners){
        this.numCorners = corners;
    }
     public const float outerRadius = 10f;
    public  float XOffset { get{
        return outerRadius*Mathf.Sin(Angle);
    } }
    public  float InnerRadius { get{
        return  outerRadius*Mathf.Cos(Angle);
    } }
    public const float height = 3f;
    public int numCorners=12;
    public float Angle { get{
        return ((360)/numCorners)*Mathf.PI/180;
    } }
    private  bool oriented = false;
    private  List<Vector3> _corners = new List<Vector3>();
    public  List<Vector3>  Corners { get{
        if(!_corners.Any()){
            int start = (int) (oriented? 0f: -1);
            Debug.Log("start: "+start);
            float _angle = 0;
            for(int i=0; i< numCorners; i++){
                _corners.Add(new Vector3( start+outerRadius*Mathf.Sin(_angle), 0f,  outerRadius*Mathf.Cos(_angle)));
                _angle+=Angle;
            }
            
            // _corners.Add(new Vector3( start+outerRadius*Mathf.Sin(i), height,  outerRadius*Mathf.Cos(i)));
            _corners.Add(new Vector3(start, 0f, outerRadius));
        }
        return _corners;
    } }

}

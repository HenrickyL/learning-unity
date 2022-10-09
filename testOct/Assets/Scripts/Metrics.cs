using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public Metrics(int corners, bool oriented=false){
        this.numCorners = corners;
        this.oriented = oriented;
    }
    public const float outerRadius = 10f;
    
    
    public  float XOffset { get{
        return outerRadius*Mathf.Sin(Angle/2);
    } }
    public  float ZOffset { get{
        return outerRadius*Mathf.Cos(Angle/2);
    } }
    public  float InnerRadius { get{
        return  outerRadius*Mathf.Cos(Angle);
    } }
    public int numCorners=12;

    public float Angle { get{
        return ((360)/numCorners)*Mathf.PI/180;
    } }
    private  bool oriented = false;
    private  List<Vector3> _corners = new List<Vector3>();
    public  List<Vector3>  Corners { get{
        if(!_corners.Any()){
            GenerateCorners();
        }
        return _corners;
    } }

    private void GenerateCorners(){
            float startX = !this.oriented? 0f: XOffset;
            float startZ = !this.oriented? 0f: ZOffset;
            float _angle = !this.oriented? 0f: Angle/2 ;
            var firts = new Vector3( outerRadius*Mathf.Sin(_angle), 0f,  outerRadius*Mathf.Cos(_angle));
            for(int i=0; i< numCorners; i++){
                _corners.Add(new Vector3( outerRadius*Mathf.Sin(_angle), 0f,  outerRadius*Mathf.Cos(_angle)));
                _angle+= Angle;
            }
            _corners.Add(firts);
    }

}

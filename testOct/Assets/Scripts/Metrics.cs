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
    public float AngleRect { get{
        return (90)*Mathf.PI/180;
    } }
    private  bool oriented = false;
    private  List<Vector3> _corners = new List<Vector3>();
    public  List<Vector3>  Corners { get{
        if(!_corners.Any()){
            GenerateCorners();
        }
        return _corners;
    } }
    public float offSet = 1.5f;
    private List<int> _neighborsDirections;
    public List<int> NeighborsDirections {
        get{
            if(_neighborsDirections==null && numCorners == 6){
                if(oriented){
                    _neighborsDirections = new List<int>(){
                        0, 1, 3, 4, 5, 7
                    };
                }else{
                    _neighborsDirections = new List<int>(){
                        1, 2, 3, 5, 6, 7
                    };
                }
            }
            return _neighborsDirections;
        }
    }
    private List<Vector3> _neighborsPositions;
    public List<Vector3> NeighborsPositions { 
        get{
            return _neighborsPositions;
        } }
    private void GenerateCorners(){
        if(_neighborsPositions == null)
            _neighborsPositions = new List<Vector3>();
        float startX = !this.oriented? 0f: XOffset;
        float startZ = !this.oriented? 0f: ZOffset;
        float _angle = !this.oriented? 0f: Angle/2;
        var firts = new Vector3( outerRadius*Mathf.Sin(_angle), 0f,  outerRadius*Mathf.Cos(_angle));
        for(int i=0; i< numCorners; i++){
            _corners.Add(new Vector3( outerRadius*Mathf.Sin(_angle), 0f,  outerRadius*Mathf.Cos(_angle)));
            _angle+= Angle;
        }
        _corners.Add(firts);
        //directions
        _angle = /*AngleRect - */(this.oriented? 0f: Angle/2);
        for(int i=0; i<=numCorners; i++){
            _neighborsPositions.Add(new Vector3(
                (InnerRadius*3/2 + offSet)*Mathf.Sin(_angle),
                0f,
                (InnerRadius*3/2 + offSet)*Mathf.Cos(_angle)
                ) 
            );
            _angle += Angle;
        }
    }

    public List<Vector3> GenerateNeig(Vector3 pos){
        var res = new List<Vector3>();
        var _angle = AngleRect - (this.oriented? 0f: Angle/2);
        for(int i=(int)Direction.N; i<=(int)Direction.NW; i++){
            if(!NeighborsDirections.Contains(i)){
                continue;
            }
            var neighbor = new Vector3(
                (InnerRadius*3/2 + offSet)*Mathf.Cos(_angle),
                0f,
                (InnerRadius*3/2 + offSet)*Mathf.Sin(_angle)
                );
            res.Add(pos - neighbor
            );
            _angle -= Angle;
        }
        return res;
    }
}

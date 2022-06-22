using UnityEngine;


public class HexCell : MonoBehaviour {
    [SerializeField] public HexCoordinates coordinates;
    [SerializeField] public Material material;
    [SerializeField] public bool selected = false;
    [SerializeField]HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction){
        return neighbors[(int) direction];
    }
    public void SetNeighbor (HexDirection direction, HexCell cell) {
        if(neighbors[(int)direction] != null){
            Debug.Log("already instantatate!");
        }
		neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
	}

}
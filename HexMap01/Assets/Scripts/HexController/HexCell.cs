using UnityEngine;


public class HexCell : MonoBehaviour {
    [SerializeField] public HexCoordinates coordinates;
    [SerializeField] public Material material;
    [SerializeField] public bool selected = false;
    public Color color;
}
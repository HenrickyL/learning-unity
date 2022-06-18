using System;
using UnityEngine;
using UnityEngine.UI;


public class HexGrid : MonoBehaviour
{
    [SerializeField]private Text cellLabelPrefab;
    [SerializeField]private int width = 6;
	[SerializeField]private int height = 6;
	[SerializeField]private HexCell cellPrefab;
	public Color defaultColor = Color.white;
	public Color touchedColor = Color.magenta;
	[SerializeField]public Material touchedMaterial;
	[SerializeField]public Material defaultMaterial;

    private HexCell[] cells;
	private Canvas gridCanvas;
	HexMesh hexMesh;

	void Awake () {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}
	private void Start() {
		hexMesh.Triangulate(cells);
	}
	private void Update() {
        if (Input.GetMouseButtonUp(0)) {
			HandleInput();
		}
    }
    void HandleInput(){
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(inputRay,out hit)){
           TouchCell(hit.point, touchedMaterial);
        }
    }
	
    public void TouchCell(Vector3 position, Material colorMaterial)
    {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z /2;
		HexCell cell = cells[index];
		cell.selected = !cell.selected;
		if(cell.selected){
			cell.material = colorMaterial;
		}else{
			cell.material = defaultMaterial;

		}
		hexMesh.Triangulate(cells);
		Debug.Log("touch at "+coordinates.ToString());
    }

    void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x =(x+z*0.5f -z/2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		// cell.color = defaultColor;
		cell.material = defaultMaterial;

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

	
}

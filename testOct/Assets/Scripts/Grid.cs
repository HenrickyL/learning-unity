using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public int width = 6;
	public int height = 6;
	public Cell cellPrefab;
    Cell[] cells;
    public Text cellLabelPrefab;
    Canvas gridCanvas;
	PoliMesh mesh;


    void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		mesh = GetComponentInChildren<PoliMesh>();
        gridCanvas = GetComponentInChildren<Canvas>();
		cells = new Cell[height * width];

		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}
	void Start () {
		mesh.Triangulate(cells);
	}

	private void Update() {
		if(Input.GetKeyUp(KeyCode.Space)){
			mesh.Triangulate(cells);
		}
	}

    void CreateCell (int x, int z, int i) {
		var metric = new Metrics(8,true);
		Vector3 position;
		position.x = (x + z*0.5f - z/2 + 5f)* (metric.InnerRadius * 2f);
		position.y = 0f;
		position.z = z * (Metrics.outerRadius * 1.5f );
		Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;

        Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = x.ToString() + "\n" + z.ToString();
	}

}

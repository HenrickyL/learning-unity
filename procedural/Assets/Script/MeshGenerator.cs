using UnityEngine;

public class MeshGenerator : MonoBehaviour {

    public SquareGrid squareGrid;
    public void GenerateMesh(int[,] map, float squereSize)
    {
        squareGrid = new SquareGrid(map, squereSize);
    }

    void OnDrawGizmos()
    {
        if(squareGrid != null)
        {
            for(int x=0; x< squareGrid.squereGrid.GetLength(0); x++){
                for(int y=0; y<squareGrid.squereGrid.GetLength(1); y++){
                    Gizmos.color = (squareGrid.squereGrid[x,y].topLeft.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].topLeft.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squereGrid[x,y].topRight.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].topRight.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squereGrid[x,y].bottomRight.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].bottomRight.position, Vector3.one * 0.4f);

                    Gizmos.color = (squareGrid.squereGrid[x,y].bottomLeft.active)? Color.black : Color.white;
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].bottomLeft.position, Vector3.one * 0.4f);
                    
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].centerTop.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].centerRight.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].centerBottom.position, Vector3.one * 0.15f);
                    Gizmos.DrawCube(squareGrid.squereGrid[x,y].centerLeft.position, Vector3.one * 0.15f);
                }
            }
        }
    }
    // ----------------------
    public class Node{
        public Vector3 position;
        public int vertexIndex = -1; 

        public Node(Vector3 _pos)
        {
            this.position = _pos;
        }
    }

    public class ControllNode : Node
    {
        public bool active;
        public Node above, right;
        public ControllNode(Vector3 _pos, bool _active, float squereSize) : base(_pos)
        {
            this.active = _active;
            above = new Node(this.position + Vector3.forward*squereSize/2f);
            right = new Node(this.position + Vector3.right*squereSize/2f);
        }
    }

    public class Squere
    {
        public ControllNode topLeft, topRight, bottomRight, bottomLeft;
        public Node centerTop, centerRight, centerBottom, centerLeft;

        public Squere(ControllNode _topLeft, ControllNode _topRight, ControllNode _bottomRight, ControllNode _bottomLeft)
        {
            this.topLeft = _topLeft;
            this.topRight = _topRight;
            this.bottomRight = _bottomRight;
            this.bottomLeft = _bottomLeft;

            centerTop = topLeft.right;
            centerRight = bottomRight.above;
            centerBottom = bottomLeft.right;
            centerLeft = bottomLeft.above;
        }
    }
    public class SquareGrid{
        public Squere[,] squereGrid;
        public SquareGrid(int[,] map, float squereSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squereSize;
            float mapHeight = nodeCountY * squereSize;

            ControllNode[,] controllNodes = new ControllNode[nodeCountX, nodeCountY];
            for(int x=0; x< nodeCountX; x++){
                for(int y=0; y<nodeCountY; y++){
                    Vector3 pos = new Vector3(-mapWidth/2 + x*squereSize + squereSize/2, 0, -mapHeight/2 + y*squereSize + squereSize/2);
                    controllNodes[x,y] = new ControllNode(pos, map[x,y] == 1, squereSize);
                }
            }
            squereGrid = new Squere[nodeCountX-1, nodeCountY-1];
             for(int x=0; x< nodeCountX-1; x++){
                for(int y=0; y<nodeCountY-1; y++){
                    squereGrid[x,y] = new Squere(controllNodes[x,y+1], controllNodes[x+1,y+1], controllNodes[x+1,y], controllNodes[x,y]);
                }
            }
        }
    }
}

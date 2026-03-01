using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public string seed;
    public bool useRandomSeed;

    private PlayerInputActions actions;
  

    [Range(0,100)]
    public int randomFillPerecent;
    int[,] map;

    void Start()
    {
        actions = new PlayerInputActions();
        actions.Enable();
        actions.Input.MouseDown.performed += OnLeftClick; 
        GenerateMap();
    }

    private void GenerateMap()
    {
        Debug.Log("Reload map");
        map = new int[width, height];
        RamdomFillMap();
        for(int i=0; i<5; i++)
        {
            SmoothMap();
        }
    }

    private void OnEnable() {
        if (actions != null)
        {
            actions.Input.MouseDown.performed += OnLeftClick; 
        }
    }
    private void OnDisable() { 
        if (actions != null)
            actions.Input.MouseDown.performed -= OnLeftClick; 
    }

    private void OnLeftClick(InputAction.CallbackContext context) { GenerateMap(); }

    private void RamdomFillMap()
    {
        if(useRandomSeed)
            seed = Time.time.ToString();
        System.Random pseudRandom = new System.Random(seed.GetHashCode());

        for(int x=0; x< width; x++){
            for(int y=0; y<height; y++){
                if(x==0 || x==width-1 || y==0 || y == height - 1){
                    map[x,y]=1;
                }
                else{
                    map[x,y]=(pseudRandom.Next(0,100) < randomFillPerecent)?1:0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for(int x=0; x<width; x++){
            for(int y=0; y<height; y++){
                int neigWallTiles = GetSorroundWallCount(x,y);
                if(neigWallTiles > 4)
                {
                    map[x,y]=1;
                }else if(neigWallTiles < 4)
                {
                    map[x,y]=0;
                }
            }
        }
    }

    private int GetSorroundWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int neighbourX = gridX-1; neighbourX <=gridX+1; neighbourX++){
            for(int neighbourY = gridY-1; neighbourY <=gridY+1; neighbourY++){
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
                     if (neighbourX != gridX || neighbourY != gridY) { 
                        wallCount += map[neighbourX, neighbourY]; 
                    } 
                } else { 
                    wallCount++; 
                }
            }
        }
        return wallCount;
    }

    void OnDrawGizmos()
    {
        if(map==null)return;

        for(int x=0; x< width; x++){
            for(int y=0; y<height; y++)
            {
                Gizmos.color = (map[x,y] == 1)? Color.black : Color.white;
                Vector3 pos = new Vector3(-width/2 +x + .5f,0f, -height/2 +y+.5f );
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
}

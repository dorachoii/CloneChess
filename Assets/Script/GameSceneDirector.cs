using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneDirector : MonoBehaviour
{
    public const int TILE_X = 8;
    public const int TILE_Y = 8;
    const int PLAYER_MAX = 2;

    public GameObject[] prefabTile;
    public GameObject prefabCursor;
    GameObject[,] tiles;
    UnitController[,] units;
    public List<GameObject> prefabWhiteUnits;
    public List<GameObject> prefabBlackUnits;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[TILE_X, TILE_Y];
        for (int i = 0; i < TILE_X; i++)
        {
            for (int j = 0; j < TILE_Y; j++)
            {
                float x = i - TILE_X / 2;
                float y = j - TILE_Y / 2;

                Vector3 pos = new Vector3(x, 0, y);

                int idx = (i + j) % 2;
                GameObject tile = Instantiate(prefabTile[idx], pos, Quaternion.identity);

                tiles[i, j] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

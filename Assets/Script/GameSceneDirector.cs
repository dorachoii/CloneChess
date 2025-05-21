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

    //ユニットのプレハブ（色ごと）
    public List<GameObject> prefabWhiteUnits;
    public List<GameObject> prefabBlackUnits;

    // 1＝ポーン、２＝ルーク、3＝ナイト、4＝ビショップ、５＝クイーン、6＝キング
    public int[,] unitType =
    {
        { 2,1,0,0,0,0,11,12 },
        { 3,1,0,0,0,0,11,13 },
        { 4,1,0,0,0,0,11,14 },
        { 5,1,0,0,0,0,11,15 },
        { 6,1,0,0,0,0,11,16 },
        { 4,1,0,0,0,0,11,14 },
        { 3,1,0,0,0,0,11,13 },
        { 2,1,0,0,0,0,11,12 },
    };


    void Start()
    {
        tiles = new GameObject[TILE_X, TILE_Y];
        units = new UnitController[TILE_X, TILE_Y];


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

                //　ユニット作成
                int type = unitType[i, j] % 10;
                int player = unitType[i, j] / 10;

                GameObject prefab = getPrefabUnit(player, type);
                GameObject unit = null;
                UnitController ctrl = null;

                if (null == prefab) continue;

                pos.y += 1.5f;
                unit = Instantiate(prefab);

                ctrl = unit.GetComponent<UnitController>();
                ctrl.SetUnit(player, (UnitController.TYPE)type, tile);

                units[i, j] = ctrl;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject getPrefabUnit(int player, int type)
    {
        int idx = type - 1;
        if (0 > idx) return null;

        GameObject prefab = prefabWhiteUnits[idx];

        if (1 == player) prefab = prefabBlackUnits[idx];
        return prefab;
    }
}

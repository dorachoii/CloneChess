using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // 
    public int Player;

    public TYPE Type;
    public int ProgressTurnCount;
    public Vector2Int Pos, OldPos;

    public List<STATUS> Status;

    public enum TYPE
    {
        NONE = -1,
        PAWN = 1,
        ROOK,
        KNIGHT,
        BISHOP,
        QUEEN,
        KING
    }

    public enum STATUS
    {
        NONE = -1,
        QSIDE_CASTLING = 1,
        KSIDE_CASTLING,
        EN_PASSANT,
        CHECK
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUnit(int player, TYPE type, GameObject tile)
    {
        Player = player;
        Type = type;
        MoveUnit(tile);
        ProgressTurnCount = -1;
    }

    public void SelectUnit(bool select = true)
    {
        Vector3 pos = transform.position;
        pos.y += 2;
        GetComponent<Rigidbody>().isKinematic = true;

        if (!select)
        {
            pos.y = 1.35f;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        transform.position = pos;
    }

    public void MoveUnit(GameObject tile)
    {
        SelectUnit(false);

        Vector2Int idx = new Vector2Int(
            (int)tile.transform.position.x + GameSceneDirector.TILE_X / 2,
            (int)tile.transform.position.y + GameSceneDirector.TILE_Y / 2);

        Vector3 pos = tile.transform.position;
        pos.y = 1.35f;
        transform.position = pos;

        Status.Clear();

        if (TYPE.PAWN == Type)
        {
            if (1 < Mathf.Abs(idx.y - Pos.y))
            {
                Status.Add(STATUS.EN_PASSANT);

                int dir = -1;
                if (1 == Player) dir = 1;


                Pos.y = idx.y + dir;
            }
        }

        if (TYPE.KING == Type)
        {
            if (1 < idx.x - Pos.x)
            {
                Status.Add(STATUS.KSIDE_CASTLING);
            }

            if (-1 > idx.x - Pos.x)
            {
                Status.Add(STATUS.QSIDE_CASTLING);
            }
        }

        OldPos = Pos;
        Pos = idx;

        ProgressTurnCount = 0;
    }

    public void ProgressTurn()
    {
        if (0 > ProgressTurnCount) return;
        ProgressTurnCount++;
        if (TYPE.PAWN == Type)
        {
            if (1 < ProgressTurnCount)
            {
                Status.Remove(STATUS.EN_PASSANT);
            }
        }
    }

    UnitController getEnPassantUnit(UnitController[,] units, Vector2Int pos)
    {
        foreach (var v in units)
        {
            if (null == v) continue;
            if (Player == v.Player) continue;
            if (!v.Status.Contains(STATUS.EN_PASSANT)) continue;

            if (v.OldPos == pos) return v;
        }

        return null;
    }

    public void SetCheckStatus(bool flag = true)
    {
        Status.Remove(STATUS.CHECK);
        if (flag) Status.Add(STATUS.CHECK);
    }
}

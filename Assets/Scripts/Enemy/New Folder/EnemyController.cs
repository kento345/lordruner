/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

enum Action
{
    Left,
    Right,
    Up,
    Down
}

enum CellType
{
    Empty,    // 空間
    Ground,   // ブロック
    Ladder,   // 梯子
    Bar       // ロープ（あるなら）
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] Grid baseMap;
    [SerializeField] Tilemap groundMap;
    [SerializeField] Tilemap ladderMap;
    [SerializeField] Tilemap barMap;

    Action lastAction = Action.Left;
    Vector3 moveTarget;
    bool isMoving = false;

    float fallInterval = 0.3f;
    float fallTimer = 0f;
    bool isAir = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int enemy = WorldToGrid(transform.position);

        isAir = IsAir(enemy);

        if (isAir)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallInterval)
            {
                HandleFall(enemy);
                fallTimer = 0f;
            }
            return;
        }


        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveTarget,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, moveTarget) < 0.01f)
                isMoving = false;

            return; // ← ここ超重要
        }




        Vector2Int playerPos = WorldToGrid(player.position);


        List<(Action action, Vector2Int pos)> candidates = new();

        int dy = playerPos.y - enemy.y;
        if (playerPos.y > enemy.y && CanMoveUp(enemy))
        {
            Move(Action.Up);
            lastAction = Action.Up;
            return;
        }

        // プレイヤーが下にいて、梯子で下りられるなら最優先
        if (playerPos.y < enemy.y && CanMoveDown(enemy))
        {
            Move(Action.Down);
            lastAction = Action.Down;
            return;
        }

        // 左
        if (CanMoveLeft(enemy))
            candidates.Add((Action.Left, enemy + Vector2Int.left));

        // 右
        if (CanMoveRight(enemy))
            candidates.Add((Action.Right, enemy + Vector2Int.right));

        // 上
        if (CanMoveUp(enemy))
            candidates.Add((Action.Up, enemy + Vector2Int.up));

        // 下
        if (CanMoveDown(enemy))
            candidates.Add((Action.Down, enemy + Vector2Int.down));

        if (candidates.Count == 0)
            return;
        // 最短距離になる行動を選ぶ
        Action best = candidates[0].action;
        int bestDist = Distance(candidates[0].pos, playerPos);


        foreach (var c in candidates)
        {
            int d = Distance(c.pos, playerPos);

            if (d < bestDist ||
                (d == bestDist && c.action == lastAction))
            {
                bestDist = d;
                best = c.action;
            }
        }

        lastAction = best;
        Move(best);
    }

    int Distance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int cell = baseMap.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    Vector3 GridToWorld(Vector2Int grid)
    {
        return baseMap.GetCellCenterWorld((Vector3Int)grid);
    }

    void Move(Action a)
    {
        Vector2Int delta = a switch
        {
            Action.Left => Vector2Int.left,
            Action.Right => Vector2Int.right,
            Action.Up => Vector2Int.up,
            Action.Down => Vector2Int.down,
            _ => Vector2Int.zero
        };

        Vector2Int next = WorldToGrid(transform.position) + delta;
        moveTarget = GridToWorld(next);
        isMoving = true;
    }
    CellType GetCell(Vector2Int pos)
    {
        Vector3Int cell = (Vector3Int)pos;

        if (groundMap.GetTile(cell) != null)
            return CellType.Ground;

        if (ladderMap.GetTile(cell) != null)
            return CellType.Ladder;

        if (barMap.GetTile(cell) != null)
            return CellType.Bar;

        return CellType.Empty;
    }

    bool CanMoveLeft(Vector2Int pos)
    {
        Vector2Int target = pos + Vector2Int.left;

        if (GetCell(target) == CellType.Ground)
            return false;

        // 移動先に足場があるか？
        return HasFooting(target);
    }

    bool CanMoveRight(Vector2Int pos)
    {
        Vector2Int target = pos + Vector2Int.right;

        if (GetCell(target) == CellType.Ground)
            return false;

        return HasFooting(target);
    }

    bool CanMoveUp(Vector2Int pos)
    {
        // Bar では上下移動不可
        if (GetCell(pos) == CellType.Bar)
            return false;

        return GetCell(pos) == CellType.Ladder;
    }
    bool CanMoveDown(Vector2Int pos)
    {
        // Bar では上下移動不可
        if (GetCell(pos) == CellType.Bar)
            return false;

        CellType below = GetCell(pos + Vector2Int.down);

        if (below == CellType.Ladder)
            return true;

        if (GetCell(pos) == CellType.Ladder && below != CellType.Ground)
            return true;

        return false;
    }
    bool IsAir(Vector2Int pos)
    {
        CellType current = GetCell(pos);
        CellType below = GetCell(pos + Vector2Int.down);

        if (current == CellType.Ladder) return false;
        if (current == CellType.Bar) return false;
        if (below == CellType.Ground) return false;
        if (below == CellType.Ladder) return false;

        return true;
    }
    bool HasFooting(Vector2Int pos)
    {
        CellType below = GetCell(pos + Vector2Int.down);
        CellType current = GetCell(pos);

        return
            below == CellType.Ground ||
            below == CellType.Ladder ||
            current == CellType.Bar ||
            current == CellType.Ladder;
    }
    void HandleFall(Vector2Int pos)
    {
        Vector2Int belowPos = pos + Vector2Int.down;
        CellType below = GetCell(belowPos);

        // ===== 着地・掴まり判定 =====

        // 地面に着地
        if (below == CellType.Ground)
        {
            SnapToCellCenter(pos);
            isAir = false;
            return;
        }

        // 梯子に入る
        if (below == CellType.Ladder)
        {
            transform.position = GridToWorld(belowPos);
            isAir = false;
            return;
        }

        // ロープに掴まる
        if (below == CellType.Bar)
        {
            transform.position = GridToWorld(belowPos);

            isAir = false;
            isMoving = false;
            fallTimer = 0f;
            lastAction = Action.Left; // 任意（安定用）

            return;
        }

        // ===== 何もなければ落下継続 =====
        transform.position = GridToWorld(belowPos);
    }
    void SnapToCellCenter(Vector2Int pos)
    {
        transform.position = GridToWorld(pos);
    }
}
*/
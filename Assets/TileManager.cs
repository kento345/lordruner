using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase temporaryTile; // 置いた瞬間のタイル
    public TileBase finalTile;     // 数秒後に変わる姿
    public float changeDelay = 3.0f;

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform pos;

    private void OnEnable()
    {
        Tilemap.tilemapTileChanged += OnTilemapChanged;
    }
    private void OnDisable()
    {
        Tilemap.tilemapTileChanged -= OnTilemapChanged;
    }

    IEnumerator TileChangeTimer(Vector3Int position)
    {
        // 指定した秒数だけ待つ
        yield return new WaitForSeconds(changeDelay);

        // 変更時に「まだそのタイルがそこにあるか」を確認（上書き消去対策）
        if (tilemap.GetTile(position) == temporaryTile)
        {
            tilemap.SetTile(position, finalTile);

            KillEnemyAtPosition(position);
        }
    }

    void KillEnemyAtPosition(Vector3Int cellPosition)
    {
        // セルの中心のワールド座標を取得
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPosition);

        // タイルのサイズ（通常1.0x1.0）より少し小さい範囲でEnemyを検知
        // 引数: (中心点, サイズ, 角度, レイヤー)
        Collider2D enemyCollider = Physics2D.OverlapBox(worldPos, new Vector2(0.8f, 0.8f), 0, enemyLayer);

        if (enemyCollider != null)
        {
            // Enemyオブジェクトを破棄
            Destroy(enemyCollider.gameObject);

            //Instantiate(enemy, pos);
            
            Debug.Log($"Enemy at {cellPosition} was destroyed!");
        }
    }

    void OnTilemapChanged(Tilemap map, Tilemap.SyncTile[] syncTiles)
    {
        if (map != tilemap) { return; }

        foreach (var syncTile in syncTiles)
        {
            if(syncTile.tile == temporaryTile)
            {
                StartCoroutine(TileChangeTimer(syncTile.position));
            }
        }
    }


}

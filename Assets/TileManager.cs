using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileBase hole;
    [SerializeField] private TileBase tile;

    private float t = 5f;

    Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }
    public void RestoreHole(Vector3Int cell)
    {
        StartCoroutine(RestoreCoroutine(cell));
    }

    IEnumerator RestoreCoroutine(Vector3Int cell)
    {
        yield return new WaitForSeconds(t);

        if (tilemap.GetTile(cell) == tile)
        {
            tilemap.SetTile(cell, tile);
        }
    }


}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerHolController : MonoBehaviour
{
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;

    private bool isPreseX = false;
    private bool isPreseZ = false;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;

    [SerializeField] private TileBase tile;

    private float t = 5f;

    private float timer;
    private int tileIndex;

    public void OnRightHole(InputAction.CallbackContext context)
    {
        if (context.performed && tilemap != null)
        {
            isPreseX = true;

        }
        if (context.canceled)
        {
            isPreseX = false;
            timer = 0f;
            tileIndex = 0;
        }
    }
    public void OnLeftHole(InputAction.CallbackContext context)
    {
        if (context.performed && tilemap != null)
        {
            isPreseZ = true;
        }
        if (context.canceled)
        {
            isPreseZ = false;
            timer = 0f;
            tileIndex = 0;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPreseZ)
        {
            ChangeTile(obj1.transform.position);
        }
        else if (isPreseX)
        {
            ChangeTile(obj2.transform.position);
        }
    }
    void ChangeTile(Vector3 worldPos)
    {
        timer += Time.deltaTime;

        if (timer >= 0.2f)
        {
            timer = 0f;

            Vector3Int cell = tilemap.WorldToCell(worldPos);
            if (tilemap.HasTile(cell))
            {
                tilemap.SetTile(cell, tiles[tileIndex]);
                if (tileIndex < tiles.Length - 1)
                {
                    tileIndex = tileIndex + 1;
                }
                if (tileIndex > tiles.Length)
                {
                    t -= Time.deltaTime;
                    if (t <= 0.1f)
                    {
                        t = 0f;

                    }
                }
            }
        }
    }
}

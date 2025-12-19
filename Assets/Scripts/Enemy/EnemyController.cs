using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private TileBase targetTile;
    private CircleCollider2D colide;

    float fallInterval = 0.3f; // 何秒ごとに1マス落ちるか
    float fallTimer = 0f;

    //private bool isradder, isbar , isgold , isAir, isGround , isGround1 = false;

   [SerializeField] private bool isradder = false;
   [SerializeField] private bool isbar = false;
   [SerializeField] private bool isgold = false;
   [SerializeField] private bool isAir = false;
   [SerializeField] private bool isGround = false;
   [SerializeField] private bool isGround1 = false;
   [SerializeField] private GameObject hit = null;
   
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        colide = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        //Move();


        /*if (!isGround && !isradder && !isbar && !isGround1)
        {
            isAir = true;
        }
        else if (isGround || isradder || isbar || isGround1)
        {
            isAir = false;
        }

        if (isAir)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallInterval)
            {
                transform.position += Vector3.down * 1.0f;
                fallTimer = 0f;
            }
        }*/

        animator.SetBool("InBar", isbar);
        animator.SetBool("InRadder", isradder);
        animator.SetBool("InAir", isAir);
    }

    void FixedUpdate()
    {
        if(player == null) return;  

        Vector2 dir = (player.transform.position - transform.position).normalized;
        
        if(isGround||isGround1||isbar)
        {
            dir.y = 0f;
        }
        if (isradder)
        {
            dir.x = 0f;
        }
        rb.AddForce(dir * moveSpeed);

        float mag = dir.magnitude;
        animator.SetFloat("Speed", mag);

        //回転ｘ
        if (dir.x > 0f)
        {
            //左
            sprite.flipX = false;
        }
        else if (dir.x < 0f)
        {
            //右
            sprite.flipX = true;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = true;
        }

        // Tilemap に当たったか確認
        if (collision.collider.GetComponent<TilemapCollider2D>() == null)
            return;

        // 接触した位置（最も信頼できる）
        Vector3 hitPoint = collision.contacts[0].point;

        // セル座標へ変換
        Vector3Int cellPos = tilemap.WorldToCell(hitPoint);

        // そのセルの Tile が Tile6 か？
        if (tilemap.GetTile(cellPos) == targetTile)
        {
            colide.isTrigger = true;
            // セル中央へ瞬間移動
            Vector3 center = tilemap.GetCellCenterWorld(cellPos);
            transform.position = center;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      /*  if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            hit = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = true;
            hit = collision.gameObject;
        }*/
        if (collision.gameObject.CompareTag("Bar"))
        {
            isbar = true;
            hit = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Radder"))
        {
            isradder = true;
            hit = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("gold"))
        {
            isgold = true;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
   /*     if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
            hit = null;
        }
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = false;
            hit = null;
        }*/
        if (collision.gameObject.CompareTag("Bar"))
        {
            isbar = false;
            hit = null;
        }
        if (collision.gameObject.CompareTag("Radder"))
        {
            isradder = false;
            hit = null;
        }
        if (collision.gameObject.CompareTag("gold"))
        {
            isgold = false;
        }
    }
}

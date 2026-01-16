using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed;
    private Vector2 inputVer;
    [SerializeField] private Tilemap tilemap;

    float fallInterval = 0.3f; // 何秒ごとに1マス落ちるか
    float fallTimer = 0f;

    private bool isradder, isbar, isgold, isAir, isGround, isGround1 = false;
 
    //---------
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    [SerializeField] private GameController gm;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isAir) { return; }
        inputVer = context.ReadValue<Vector2>();
        if (context.canceled)
        {
            SnapToCellCenter();
        }
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        if (context.performed && isradder)
        {
            inputVer = context.ReadValue<Vector2>();

        }
        if (context.canceled)
        {
            inputVer.y = 0;
            SnapToCellCenter();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(!isGround && !isradder && !isbar && !isGround1)
        {
           isAir = true;
        }
        else if(isGround || isradder || isbar || isGround1)
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
        }
        animator.SetBool("InBar", isbar);
        animator.SetBool("InRadder", isradder);
        animator.SetBool("InAir", isAir);
    }

    void Move()
    {
        if(isAir) {return;}
        //移動
        Vector3 move = new Vector3(inputVer.x, inputVer.y, 0f) * Speed * Time.deltaTime;
        transform.position += move;
        //----------

        float mag = inputVer.magnitude;
        animator.SetFloat("Speed", mag);
        //回転ｘ
        if (inputVer.x > 0f)
        {
            //左
            sprite.flipX = false;
        }
        else if (inputVer.x < 0f)
        {
            //右
            sprite.flipX = true;
        }
    }

    void SnapToCellCenter()
    {
        Vector3 worldPos = transform.position;

        // ワールド → セル座標
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        // セル中央 → ワールド座標
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);

        transform.position = cellCenter;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        /*if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = true;
        }*/
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
       /* if (collision.gameObject.CompareTag("Ground1"))
        {
            isGround1 = false;
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bar"))
        {
            isbar = true;
        }
        if (collision.gameObject.CompareTag("Radder"))
        {
            isradder = true;
        }
        if (collision.gameObject.CompareTag("gold"))
        {
            isgold = true;
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            gm.Score(1);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Dead");

            gm.GameOvere();
        }
        if (collision.gameObject.CompareTag("wall"))
        {
            SceneManager.LoadScene("End");
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bar"))
        {
            isbar = false;
        }
        if (collision.gameObject.CompareTag("Radder"))
        {
            isradder = false;
        }
        if (collision.gameObject.CompareTag("gold"))
        {
            isgold = false;
        }
    }
}

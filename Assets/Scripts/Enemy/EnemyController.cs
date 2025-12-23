using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Tilemaps;

public enum EnemyState
{
    Ground,
    Climbing
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float climbSpeed = 2f;

    [SerializeField] EnemyState state = EnemyState.Ground;

    [SerializeField] bool onLadder = false;
    [SerializeField] bool alignedWithLadder = false;

    void Update()
    {
        switch (state)
        {
            case EnemyState.Ground:
                GroundMove();
                break;

            case EnemyState.Climbing:
                ClimbMove();
                break;
        }
    }

    void GroundMove()
    {
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);

        // 梯子にいて、かつPlayerが上下にいる
        if (onLadder && Mathf.Abs(player.position.y - transform.position.y) > 0.5f)
        {
            // X方向が十分近い → 登る
            if (alignedWithLadder)
            {
                state = EnemyState.Climbing;
            }
        }
    }

    void ClimbMove()
    {
        float dir = Mathf.Sign(player.position.y - transform.position.y);

        // X固定・Yのみ移動
        transform.Translate(Vector2.up * dir * climbSpeed * Time.deltaTime);

        // 梯子を外れた or 同じ高さ
        if (!onLadder || Mathf.Abs(player.position.y - transform.position.y) < 0.1f)
        {
            state = EnemyState.Ground;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Radder"))
        {
            onLadder = true;

            // 梯子の中心に近ければ true
            float dx = Mathf.Abs(collision.bounds.center.x - transform.position.x);
            alignedWithLadder = dx < 0.1f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Radder"))
        {
            float dx = Mathf.Abs(collision.bounds.center.x - transform.position.x);
            alignedWithLadder = dx < 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Radder"))
        {
            onLadder = false;
            alignedWithLadder = false;
        }
    }
}


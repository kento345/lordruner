using UnityEngine;

public class Colide : MonoBehaviour
{
    [SerializeField] bool onLadder = false;
    [SerializeField] bool onGropund = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGropund = true;
        }
        if (collision.CompareTag("Ground1"))
        {
            onGropund = true;
        }
        if (collision.CompareTag("Radder"))
        {
            onLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGropund = false;
        }
        if (collision.gameObject.CompareTag("Ground1"))
        {
            onGropund = false;
        }
        if (collision.CompareTag("Radder"))
        {
            onLadder = false;
        }
    }
    public(bool OnLadder,bool OnGropund) ChackState()
    {
        return (onLadder,onGropund);
    }
}

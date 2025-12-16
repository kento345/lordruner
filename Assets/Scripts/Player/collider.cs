using UnityEngine;

public class collider : MonoBehaviour
{
    public bool istile1 = false;
    public bool istile2 = false;
    private GameObject tile1;
    private GameObject tole2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground1"))
        {
            istile1 = true;
            tile1 = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground1"))
        {
            istile1 = false;
            tile1 = null;
        }
    }

    public GameObject Chack()
    {
        if(istile1)
        {
            return tile1;
        }

        return null;
    }
}

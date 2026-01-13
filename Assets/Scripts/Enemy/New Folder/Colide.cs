using UnityEngine;

public class Colide : MonoBehaviour
{
    [SerializeField] bool onhole = false;
    //[SerializeField] bool onGropund = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
    }
/*    public(bool OnLadder,bool OnGropund) ChackState()
    {
        return (onLadder,onGropund);
    }*/
}

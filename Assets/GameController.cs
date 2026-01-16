using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SpriteRenderer player;

    [SerializeField] private GameObject goal;



    private int count;

    private void Start()
    {
        panel.SetActive(false);
    }


    public void GameOvere()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        for(int i = 0; i < 3; i++)
        {
            player.enabled = !player.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Score(int x)
    {
        count += x;

        if(count == 3)
        {
           goal.SetActive(true);
            Debug.Log("100");
        }
    }
}

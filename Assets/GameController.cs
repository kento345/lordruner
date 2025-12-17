using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SpriteRenderer player;

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
        for(int i = 0; i < 2; i++)
        {
            player.enabled = !player.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        panel.SetActive(true);
        //yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

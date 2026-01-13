using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{

    public void StartButton()
    {
        SceneManager.LoadScene("Main");
    }
}

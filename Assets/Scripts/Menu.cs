using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu FirstInstance;

    public Button startButton;
    public TMP_InputField register_username;
    public string userName;

    private void Awake()
    {
        /*if (FirstInstance != null)
        {
            Destroy(gameObject);
            return;
        }*/

        FirstInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (register_username.text != "")
        {
            SceneManager.LoadScene(1);
            userName = register_username.text;
        }
        else {Debug.Log("Please enter name"); }
    }


}
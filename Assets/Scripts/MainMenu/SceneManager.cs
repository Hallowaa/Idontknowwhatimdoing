using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas settingsCanvas;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas.enabled = true;
        settingsCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName: "MainGame");
    }

    public void GoToSettingsMenu()
    {
        Invoke("TurnOnSettingsCanvas", 1.30f);
        Camera.main.GetComponent<Animator>().SetTrigger("TransferToSettings");
        mainCanvas.enabled = false;
    }
    
    private void TurnOnSettingsCanvas()
    {
        settingsCanvas.enabled = true;
    }
}

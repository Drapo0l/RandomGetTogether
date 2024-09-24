using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonfuntionsS : MonoBehaviour
{
    public void Resume() 
    {
        GameManager.Instance.unpausedState();
    }
    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.unpausedState();
    }

    public void Quit() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
   

    public void Start_Button()
    {
        GameManager.Instance.Player_HP_Bar.enabled = true;
        GameManager.Instance.AmmoC.enabled = true;
        GameManager.Instance.AmmoM.enabled = true;
        GameManager.Instance.unpausedState();
        GameManager.Instance.Menu_Start.SetActive(false);
    }
}

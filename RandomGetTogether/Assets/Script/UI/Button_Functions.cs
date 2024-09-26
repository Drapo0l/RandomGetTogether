using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;


public class ButtonfuntionsS : MonoBehaviour
{
    public void Resume() 
    {
        GameManager.Instance.playerADU.PlayOneShot(GameManager.Instance.AUDclick[Random.Range(0, GameManager.Instance.AUDclick.Length)], GameManager.Instance.AUDclickV);
        GameManager.Instance.unpausedState();
    }
    public void Restart() 
    {
        GameManager.Instance.playerADU.PlayOneShot(GameManager.Instance.AUDclick[Random.Range(0, GameManager.Instance.AUDclick.Length)], GameManager.Instance.AUDclickV);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.unpausedState();
    }

    public void Quit() 
    {
        GameManager.Instance.playerADU.PlayOneShot(GameManager.Instance.AUDclick[Random.Range(0, GameManager.Instance.AUDclick.Length)], GameManager.Instance.AUDclickV);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
   

    public void Start_Button()
    {
        GameManager.Instance.playerADU.PlayOneShot(GameManager.Instance.AUDclick[Random.Range(0, GameManager.Instance.AUDclick.Length)], GameManager.Instance.AUDclickV);
        GameManager.Instance.Menu_Start.SetActive(false);
        GameManager.Instance.A_Player_hp_bar.SetActive(true);
        GameManager.Instance.ammoBox.SetActive(true);
        GameManager.Instance.gold_box.SetActive(true);
        GameManager.Instance.unpausedState();
       
    }
}

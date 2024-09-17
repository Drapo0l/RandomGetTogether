using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManeger : MonoBehaviour
{
    public static GameManeger Instance;

    [SerializeField] GameObject MenuActive;
    [SerializeField] GameObject MenuPause;
    [SerializeField] GameObject MenuLose;
    [SerializeField] GameObject MenuWin;
    [SerializeField] TMP_Text enemyCountText;
    public Image playerHPBar;

    public GameObject dmgPanel;
    public GameObject player;
    public PlayerMovement playerScript;

    int enemyCount;
    float timeScaleOriginal;
    public bool isPause;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        timeScaleOriginal = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MenuActive == null)
            {
                statepause();
                MenuActive = MenuPause;
                MenuActive.SetActive(isPause);

            }
            else if (MenuActive == MenuPause)
            {
                stateUnpause();
            }

        }
    }

    public void statepause()
    {
        isPause = !isPause;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPause = !isPause;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        MenuActive.SetActive(isPause);
        MenuActive = null;
    }



    public void UpdateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        if (enemyCount <= 0)
        {
            //player wins the game
            //Debug.Log("You Win!");
            statepause();
            MenuActive = MenuWin;
            MenuWin.SetActive(isPause);

        }
    }

    public void youLose()
    {
        statepause();
        MenuActive = MenuLose;
        MenuActive.SetActive(true);
    }
}

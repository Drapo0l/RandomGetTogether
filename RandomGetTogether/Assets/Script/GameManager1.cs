using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class GameManager : MonoBehaviour   
{
    [SerializeField] GameObject Menu_Active; 
    [SerializeField] GameObject Menu_Win; 
    [SerializeField] GameObject Menu_Pause; 
    [SerializeField] GameObject Menu_Lose; 
    public Image Player_HP_Bar; 
    [SerializeField] TMP_Text EnemyCount_Text; 

    public GameObject DMG_Screen; 
    public static GameManager Instance;
    public GameObject Player;
    public PlayerMovement PlayerScript;
    public CustomBullet Player_Dmg;
    int Enemy_Count;
    public bool paused;  
    float timeScale_OG; 
    public GameObject TeleportAnchor;  
 
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
       
        Instance = this;
        timeScale_OG = Time.timeScale;
        Player = GameObject.FindWithTag("Player");
        TeleportAnchor = GameObject.FindWithTag("SpawnPoint"); 
        PlayerScript = Player.GetComponent<PlayerMovement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) 
        {
            if (Menu_Active == null)
            {
                pausedState();
                Menu_Active = Menu_Pause;
                Menu_Active.SetActive(paused);
            }
            else if (Menu_Active == Menu_Pause) 
            {
                unpausedState();
            }
        }
    }

    public void pausedState() 
    {
        paused = !paused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpausedState() 
    {
        paused = !paused;
        Time.timeScale = timeScale_OG;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Menu_Pause.SetActive(paused);
        Menu_Active = null;
    }

    public void updateGgoal(int ammount) 
    {
        Enemy_Count += ammount; 
        EnemyCount_Text.text = Enemy_Count.ToString("F0"); 
        if (Enemy_Count <= 0)
        {
            
            pausedState();
            Menu_Active = Menu_Win;
            Menu_Active.SetActive(true); 
        }
        if(PlayerScript.health == 0)
        {
            Defeat();
        }
    }
    public void Defeat()
    {
        pausedState();
        Menu_Active = Menu_Lose;
        Menu_Active.SetActive(true);
    }
   public IEnumerator dmgflash()
    {
        DMG_Screen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        DMG_Screen.SetActive(false);
    }
}

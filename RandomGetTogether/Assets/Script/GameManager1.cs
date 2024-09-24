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
    public Image Gdmg;
    public GameObject Menu_Start;
    public Image Player_HP_Bar;
    public TMP_Text AmmoC,AmmoM;
    public TMP_Text GoldC;



    public GameObject DMG_Screen; 
    public static GameManager Instance;
    public GameObject Player;
    public PlayerMovement PlayerScript;
    public CustomBullet Player_Dmg;
    int Enemy_Count;
    public bool paused;  
    float timeScale_OG; 
    public GameObject TeleportAnchor;
    public int gold;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
       
        Instance = this;
        timeScale_OG = Time.timeScale;
        Player = GameObject.FindWithTag("Player");
        TeleportAnchor = GameObject.FindWithTag("SpawnPoint"); 
        PlayerScript = Player.GetComponent<PlayerMovement>();
        //pausedState(); //commented so game does not start paused


    }
    void Start()
    {
       
    }
    public void UpdateHealthBar()
    {

        if (PlayerScript != null)
        {

            // Assuming PlayerScript has a 'health' and 'maxHealth' variable
            float healthPercentage = PlayerScript.health / PlayerScript.maxHealth;
            Player_HP_Bar.fillAmount = healthPercentage; // Updates the health bar fill amount
            Gdmg.color = new Color(255, 0, 0, 100 - healthPercentage);
            GoldC.text = gold.ToString("F0");
            
        }
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
        UpdateHealthBar();
       
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

    public void updateGgoal() 
    {
      
        if(Player.GetComponent<PlayerMovement>().health <=0)
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

    public void Win()
    {
        pausedState();
        Menu_Active = Menu_Win;
        Menu_Active.SetActive(true);
    }
   public IEnumerator dmgflash()
    {
        
        DMG_Screen.SetActive(true);
        yield return new WaitForEndOfFrame();
        DMG_Screen.SetActive(false);
    }
}

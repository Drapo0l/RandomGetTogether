using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;




public class GameManager : MonoBehaviour   
{
    [Header("Menues")]
    [SerializeField] GameObject Menu_Active; 
    [SerializeField] GameObject Menu_Win; 
    [SerializeField] GameObject Menu_Pause; 
    [SerializeField] GameObject Menu_Lose;
    [SerializeField] GameObject GdmgB;

    [Header("-----Shop Fields-----")]
    [SerializeField] public GameObject playerPrompt;
    [SerializeField] public TMP_Text promptText;
    [SerializeField] public GameObject Inventory;


    [Header("Menu objects")]
    public Image Gdmg;
    public GameObject Menu_Start;
    public Image Player_HP_Bar;
    public TMP_Text AmmoC;
    public TMP_Text GoldC;
    public TMP_Text hp_text;
    public GameObject ammoBox;
    public GameObject gold_box;
    public GameObject A_Player_hp_bar;
    public GameObject DMG_Screen;
    [Header("other")]
    public static GameManager Instance;
    public GameObject Player;
    public PlayerMovement PlayerScript;
    public CustomBullet Player_Dmg;
    //int Enemy_Count;
    public bool paused;  
    float timeScale_OG; 
    public GameObject TeleportAnchor;
    public int gold = 0;
    //int enemyCount;
    float healthPercentage;
    float safe;
    [Header("audio")]
    public AudioClip[] AUDclick; 
    [SerializeField] float AUDclickV;


    // Start is called before the first frame update
    void Awake()
    {
      
        Instance = this;
        timeScale_OG = Time.timeScale;
        Player = GameObject.FindWithTag("Player");
        TeleportAnchor = GameObject.FindWithTag("SpawnPoint"); 
        PlayerScript = Player.GetComponent<PlayerMovement>();
      


    }
    void Start()
    {
        //StartG();
    }
    public void UpdateHealthBar()
    {
        Gdmg.color = new Color(255, 0, 0, 0);
        if (PlayerScript != null)
        {
            healthPercentage = 0;
            safe = 0;
           hp_text.text = Player.GetComponent<PlayerMovement>().health.ToString("F0");
            // Assuming PlayerScript has a 'health' and 'maxHealth' variable
            healthPercentage = Player.GetComponent<PlayerMovement>().health / Player.GetComponent<PlayerMovement>().maxHealth;
             safe = 70/100;
            safe = 1 - safe;
            safe = safe / 3;
            Player_HP_Bar.fillAmount = healthPercentage; // Updates the health bar fill amount
            healthPercentage = 1 - healthPercentage;
            healthPercentage = healthPercentage / 3;
            if (healthPercentage < safe)
            {
                Gdmg.color = new Color(255, 0, 0, healthPercentage);
            }
            else
            {
                Gdmg.color = new Color(255, 0, 0, safe);
            }
           
            //GoldC.text = gold.ToString("F0");
            
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
    public void StartG()
    {
        pausedState();
        Menu_Active = Menu_Start;
        Menu_Active.SetActive(true);
    }
    public void pausedState() 
    {
        GdmgB.SetActive(false);
        Player.SetActive(false);
        paused = !paused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpausedState() 
    {
        GdmgB.SetActive(true);
        Player.SetActive(true);
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
            GdmgB.SetActive(false);
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

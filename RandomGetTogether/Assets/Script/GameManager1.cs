using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject Player;
    public PlayerMovement PlayerScript;        
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Player = GameObject.FindWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerMovement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGameGoal(int amount)
    {
        enemyCount += amount;

        if (enemyCount <= 0) 
        {
            //player wins the game
            Debug.Log("You Win!!!");
        }
    }
}

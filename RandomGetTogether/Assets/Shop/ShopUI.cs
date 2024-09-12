using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] Canvas StoreFront;
    [SerializeField] GameObject playerPrompt;
    [SerializeField] GameObject[] shopItems;

   public bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        StoreFront.gameObject.SetActive(false);
        playerPrompt.SetActive(false);
        //Make sure the shop is inactive
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.RightShift))
        {
            CloseShop();
        }  

        else if (!isOpen && Input.GetKeyDown(KeyCode.RightShift))
        {
            OpenShop();
        } 
        //Check for input to open or close the shop


    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(true);
        }
    }//Show prompt if shop is not open

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(false);
        }
        //Show prompt when player enters trigger area
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(false);
        }
        //Hides prompt when player exits trigger area
    }


    private void OpenShop()
    {
        isOpen = true;
        StoreFront.gameObject.SetActive(true);
        Time.timeScale = 0;
        playerPrompt.SetActive(false);
    }// show shop


    private void CloseShop()
    {
        isOpen = false;
        StoreFront.gameObject.SetActive(false);
        Time.timeScale = 1;
        
    }//Hide shop
}

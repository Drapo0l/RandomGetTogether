using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] Canvas StoreFront;
    [SerializeField] GameObject playerPrompt;
    [SerializeField] GameObject StoreMenu;
    [SerializeField] GameObject Shop;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject InventorySlot;
    [SerializeField] shopItems[] items;
    
    

   bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        StoreFront.gameObject.SetActive(false);
        playerPrompt.SetActive(false);
        showShop();
        
        //Make sure the shop is inactive
    }

    void showShop()
    {
        for (int i = StoreMenu.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(StoreMenu.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < items.Length; i++)
        {
            GameObject slot = Instantiate(InventorySlot, StoreMenu.transform);

        }
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

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(true);
        }
    }//Show prompt if shop is not open

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(false);
        }
        //Show prompt when player enters trigger area
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerPrompt.SetActive(false);
        }
        //Hides prompt when player exits trigger area
    }


     void OpenShop()
    {
        isOpen = true;
        StoreFront.gameObject.SetActive(true);
        Time.timeScale = 0;
        playerPrompt.SetActive(false);
    }// show shop


     void CloseShop()
    {
        isOpen = false;
        StoreFront.gameObject.SetActive(false);
        Time.timeScale = 1;
        
    }//Hide shop
}

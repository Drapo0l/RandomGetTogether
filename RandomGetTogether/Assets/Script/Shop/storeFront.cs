using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class storeFront : MonoBehaviour
{
    [SerializeField] GameObject playerPrompt;
    [SerializeField] TMP_Text promptText;

    // Start is called before the first frame updateSB
    void Start()
    {

        playerPrompt = GameManager.Instance.playerPrompt;
        promptText = GameManager.Instance.promptText;
        playerPrompt.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                playerPrompt.SetActive(true);
                promptText.SetText("Press E to Shop");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        playerPrompt.SetActive(false);
    }
}

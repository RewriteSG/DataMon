using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : MonoBehaviour
{
    public string Text;
    public GameObject ConversationUI;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !GameManager.instance.isInteractingNPC)
            {
                GameManager.instance.isInteractingNPC = true;
                ConversationUI.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void ResetTimeScale()
    {
        Time.timeScale = 1;
        GameManager.instance.isInteractingNPC = false;
    }
}

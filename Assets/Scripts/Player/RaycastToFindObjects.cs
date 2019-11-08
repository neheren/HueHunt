using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastToFindObjects : MonoBehaviour
{

    public bool findObject = false;
    public Text interactPanelText;
    private LayerMask mask;

   
    private void Start()    
    {
        mask = LayerMask.GetMask("InteractableItem");
    }

    void Update()
    {
        if (findObject)
        {
            RaycastHit hit;
            //Raycast to try find objects in layer 10 (interactableItems)
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
            {
                GameObject go = hit.transform.gameObject;

                interactPanelText.text = "Press 'F' to interact with " + go.name;
                MonoBehaviour[] list = go.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour mb in list)
                {
                    if (mb is ItemInterface && (Input.GetKeyDown(KeyCode.F)))
                    {
                        ItemInterface item = (ItemInterface)mb;
                        item.ItemEnteractedWith();
                    }
                }
            }

            else{
                interactPanelText.text = string.Empty;
            }
        }
        else if (interactPanelText.text != string.Empty)
            interactPanelText.text = string.Empty;
    }




    
}

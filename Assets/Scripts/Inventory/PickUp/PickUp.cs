using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickUp{
    
    private Inventory inventory;
    private GameObject itemButton;

    private void Start()
    {
        inventory = GameObject.FindObjectWithTag("Player").GetComponent<Inventory>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false)
                {
                    //picking up item
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, Inventory.slots[i].transform, false);
                    Destroy(GameObject);
                    break;
                }
            }
        }
    }
}

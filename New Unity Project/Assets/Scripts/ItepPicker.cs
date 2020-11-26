using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItepPicker : MonoBehaviour
{
    [SerializeField] private int _healValue;
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<Player_Controller>().ChangeHp(_healValue); 
       Destroy(gameObject);   
    }
}

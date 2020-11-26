using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] protected Slider _hpSlider;
    private int _currentHP;
    Animator _anienemy;

    void Start()
    {
        _anienemy = GetComponent<Animator>();
        _currentHP = _hp;
        _hpSlider.maxValue = _hp;
        _hpSlider.value = _hp;
    }


    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        
        if (damage > 0)
            _anienemy.SetBool("Hitted", true);
        if (_currentHP <= 0)
        {
            _anienemy.SetBool("Death", true);
        }

        Debug.Log("Damage - " + damage);
        Debug.Log("HP - " + _currentHP);
        _hpSlider.value = _currentHP;

    }
    public void UndoHit()
    {
        _anienemy.SetBool("Hitted", false);
    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }
}

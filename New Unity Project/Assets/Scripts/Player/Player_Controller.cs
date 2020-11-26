using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    private ServiseManager serviceManager;
    [SerializeField] protected Slider _hpSlider;
    [SerializeField] protected Slider _mpSlider;
    [SerializeField] private int _maxHP;
    private int _currentHp;
    [SerializeField] private int _maxMP;
    private int _currentMP;
    Animator _playeranim;

    void Start()
    {
        _playeranim = GetComponent<Animator>();
        _currentHp = _maxHP;
        _currentMP = _maxMP;
        _hpSlider.maxValue = _maxHP;
        _hpSlider.maxValue = _maxHP;
        _mpSlider.value = _maxMP;
        _mpSlider.value = _maxMP;
        serviceManager = ServiseManager.Instanse;
    }

    public void ChangeHp(int value)
    {

        _currentHp += value;
        if (value < 0)
            _playeranim.SetBool("Hitted", true);
        if (_currentHp <= 0)
        {
            _playeranim.SetBool("Death", true);
        }
        if (_currentHp > _maxHP)
        {
            _currentHp = _maxHP;
        }
        if (value != 0)
        {
            Debug.Log("value - " + value);
            Debug.Log("Current HP - " + _currentHp);
        }
        _hpSlider.value = _currentHp;
    }
    public void UndoHit()
    {
        _playeranim.SetBool("Hitted", false);
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        _playeranim.SetBool("Hitted", true);
        if (_currentHp <= 0)
        {
            _playeranim.SetBool("Death", true);
        }
        Debug.Log("value - " + damage);
        Debug.Log("Current HP - " + _currentHp);
        _hpSlider.value = _currentHp;
    }

    public bool ChangeMP(int value)
    {
        if (_currentMP > _maxMP)
            _currentMP = _maxMP;
        if (value > 0 || _currentMP < (-value))
            return false;

        _currentMP += value;
        Debug.Log("MP value = " + value);
        Debug.Log("Current MP - " + _currentMP);
        _mpSlider.value = _currentMP;
        return true;
    }

    public void OnDeath()
    {
        serviceManager.Restart();
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectibleController : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _timeDelay;
    private DateTime _lastEncounter;

    private void OnTriggerEnter2D(Collider2D info)
    {
        if ((DateTime.Now - _lastEncounter).TotalSeconds < 0.1f)
            return;
        EnemiesController enemy = info.GetComponent<EnemiesController>();
        if(enemy != null)
        {
            enemy.TakeDamage(_damage);
        }
        Player_Controller _player = info.GetComponent<Player_Controller>();

        if (_player != null && (DateTime.Now - _lastEncounter).TotalSeconds > _timeDelay)
        {
            _player.ChangeHp(-_damage);
            _lastEncounter = DateTime.Now;
        }
    }
}

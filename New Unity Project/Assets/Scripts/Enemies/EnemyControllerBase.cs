    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public abstract class EnemyControllerBase : MonoBehaviour
{
    protected Rigidbody2D _enemyRb;
    protected Animator _enemyAnimator;

    [Header("StateChanges")]
    [SerializeField] private float _maxStateTime;
    [SerializeField] private float _minStateTime;
    [SerializeField] private EnemyState[] _availableState;
    protected EnemyState _currentState;
    protected float _lastStateChange;
    protected float _timeToNextChange;

    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    protected Vector2 _startPoint;
    protected bool _faceRight = true;
    

    protected virtual void Start()
    {
        _startPoint = transform.position;
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
    
        if (IsGroundEnding())
            Flip();

        if (_currentState == EnemyState.Move)
            Move();
    }

    protected virtual void Update()
    {
         if (Time.time - _lastStateChange > _timeToNextChange)
            GetRandomState();
    }

    protected virtual void Move()
    {
        _speed = 1.0f;
        _enemyRb.velocity = transform.right * new Vector2(_speed, _enemyRb.velocity.y);
    }

    protected void Flip()
    {    
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_range * 2, 0.5f, 0));
    }

    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(_groundCheck.position, _whatIsGround);
    }

    protected void GetRandomState()
    {
        int state = Random.Range(0, _availableState.Length);

        if (_currentState == EnemyState.Idle && _availableState[state] == EnemyState.Idle)
        {
            GetRandomState();
        }

        _timeToNextChange = Random.Range(_minStateTime, _maxStateTime);
        ChangeState(_availableState[state]);
    }

    protected virtual void ChangeState(EnemyState state)
    {
        /* if (state != EnemyState.Idle)
         {
             _enemyAnimator.SetBool(state.ToString(), true);
             if(_speed > 0.1)
             {
                 _enemyAnimator.SetBool("IsRunning", true);
             }
             else
             {
                 _enemyAnimator.SetBool("IsRunning", false);
             }  

         }         
         if (_currentState != EnemyState.Idle)
         {
             _enemyAnimator.SetBool(_currentState.ToString(), false);
             _enemyAnimator.SetBool("IsRunning", false);
             _enemyAnimator.SetBool("Shoot", true);
         }*/
   if (state == EnemyState.Move)
   {
           _enemyAnimator.SetBool("IsRunning", true);
   }
   else
   {
           _enemyAnimator.SetBool("IsRunning", false);
   }
   if (state == EnemyState.Strike)
   {
       _enemyAnimator.SetBool("Strike", true);
   }
   else
   {
       _enemyAnimator.SetBool("Strike", false);
   }
   if (state == EnemyState.PowerStrike)
   {
       _enemyAnimator.SetBool("PowerStrike", true);
   }
   else
   {
       _enemyAnimator.SetBool("PowerStrike", false);
   }
        if (state == EnemyState.Shoot)
        {
            _enemyAnimator.SetBool("Shoot", true);
        }
        else
        {
            _enemyAnimator.SetBool("Shoot", false);
        }


        _currentState = state;
   _lastStateChange = Time.time;
}

protected virtual void EndState()
{
   ResetState();
}

protected virtual void ResetState()
{
   _enemyAnimator.SetBool(EnemyState.Move.ToString(), false);
   _enemyAnimator.SetBool(EnemyState.Death.ToString(), false);
}
}

public enum EnemyState
{
Idle,
Move,
Shoot,
Strike,
PowerStrike,
Hurt,
Death,
}
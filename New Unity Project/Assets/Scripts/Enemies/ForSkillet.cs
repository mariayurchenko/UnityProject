using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForSkillet : EnemyControllerBase
{
  /*  public override void TakeDamage(int damage, DamageType type = DamageType.Casual, Transform palyer = null)
    {
        if (type != DamageType.Projectile)
            return;

        base.TakeDamage(damage, type, palyer);
    }*/
    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (_currentState)
        {
            case EnemyState.Idle:
                _enemyRb.velocity = Vector2.zero;
                break;
            case EnemyState.Move:
                _startPoint = transform.position;
                break;
            
        }
    }
}

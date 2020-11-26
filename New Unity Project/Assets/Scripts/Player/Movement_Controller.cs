using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement_Controller : MonoBehaviour
{
    private Rigidbody2D _playerRB;
    private Animator _playerAnimator;
    [Header("Horizontal Movements")]
    [SerializeField] private float _speed;

    private bool _faceRight = true;
    private bool _canMove = true;
    private Player_Controller _playerController;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _radius;
    [SerializeField] private bool _airControll;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _whatIsGround;
    private bool _grounded;

    [Header("Crawling")]
    [SerializeField] private Transform _cellCheck;
    [SerializeField] private CircleCollider2D _headCollider;
    private bool _canStand;

    [Header("Casting")]
    [SerializeField] private GameObject _fireBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireBallSpeed;
    [SerializeField] private int _castCost;
    private bool _isCasting;

    [Header("Strike")]
    [SerializeField] private Transform _strikePoint;
    [SerializeField] private int _damage;
    [SerializeField] private float _strikeRange;
    [SerializeField] private LayerMask _enemies;
    private bool _isStriking;

    [Header("PowerStrike")]
    [SerializeField] private float _chargeTime;
    public float ChargeTime => _chargeTime;
    [SerializeField] private float _powerStrikeSpeed;
    [SerializeField] private Collider2D _strikeCollider;
    [SerializeField] private int _powerStrikeDamage;
    [SerializeField] private int _powerStrikeCost;
    private List<EnemiesController> _damagedEnemies = new List<EnemiesController>();

    void Start()
    {
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerController = GetComponent<Player_Controller>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_cellCheck.position, _radius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_strikePoint.position, _strikeRange);
    }

    private void OnColliderEnter2D(Collision2D collision)
    {
        if (!_strikeCollider.enabled)
        {
            return;
        }

        EnemiesController enemy = collision.collider.GetComponent<EnemiesController>();
        if (enemy == null || _damagedEnemies.Contains(enemy))
            return;

        enemy.TakeDamage(_powerStrikeDamage);
        _damagedEnemies.Add(enemy);
    }

    void Flip()
    {
        _faceRight = !_faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void Move(float move, bool jump, bool crawling)
    {
        if (!_canMove)
            return;
        #region Movement
        
        if (move != 0 && (_grounded || _airControll))
            _playerRB.velocity = new Vector2(_speed * move, _playerRB.velocity.y);

        if (move > 0 && !_faceRight)
        {
            Flip();
        }
        else if (move < 0 && _faceRight)
        {
            Flip();
        }
        #endregion
        #region Jumpimg
        _grounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _whatIsGround);
        if (jump && _grounded)
        {
            _playerRB.AddForce(Vector2.up * +_jumpForce);
            jump = false;
        }
        #endregion
        #region Crawling
        _canStand = !Physics2D.OverlapCircle(_cellCheck.position, _radius, _whatIsGround);
        if (crawling)
        {
            _headCollider.enabled = false;
            
        }
        else if (!crawling && _canStand)
        {
            _headCollider.enabled = true;
        }
        
        #endregion
        #region Animation
        _playerAnimator.SetFloat("Speed", Mathf.Abs(move));
        _playerAnimator.SetBool("Jump", !_grounded);
        _playerAnimator.SetBool("Crouch", !_headCollider.enabled);
        #endregion
    }

    public void StartCasting()
    {
        _isCasting = true;
        _playerAnimator.SetBool("Casting", true);
    }

    private void CastFire()
    {
        if (!_isCasting || !_playerController.ChangeMP(-_castCost))
            return;
        GameObject fireBall = Instantiate(_fireBall, _firePoint.position, Quaternion.identity);
        fireBall.GetComponent<Rigidbody2D>().velocity = transform.right * _fireBallSpeed;
        fireBall.GetComponent<SpriteRenderer>().flipX = !_faceRight;
        Destroy(fireBall, 5f);
    }

    private void EndCasting()
    {
        _isCasting = false;
        _playerAnimator.SetBool("Casting", false);
    }

    public void StartStrike(float holdTime)
    {
        if (_isStriking)
            return;
        if(holdTime >= _chargeTime)
        {
            if (!_playerController.ChangeMP(-_powerStrikeCost))
                return;
            _playerAnimator.SetBool("PowerStrike", true);
            _canMove = false;
        }
        else
        {
            _playerAnimator.SetBool("Strike", true);
        }
        _isStriking = true;
    }

    private void StartPowerStrike()
    {
        _playerRB.velocity = transform.right * _powerStrikeSpeed;
        _strikeCollider.enabled = true;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemiesController enemy = enemies[i].GetComponent<EnemiesController>();
            enemy.TakeDamage(_powerStrikeDamage);
        }
        
    }

    private void DisablePowerStrike()
    {
        _playerRB.velocity = Vector2.zero;
        _strikeCollider.enabled = false;
        _damagedEnemies.Clear();
    }

    private void EndPowerStrike()
    {
        _playerAnimator.SetBool("PowerStrike", false);
        _canMove = true;
        _isStriking = false;
    }

    private void Strike()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_strikePoint.position, _strikeRange, _enemies);
        for(int i = 0; i < enemies.Length; i++)
        {
            EnemiesController enemy = enemies[i].GetComponent<EnemiesController>();
            enemy.TakeDamage(_damage);
        }
    }

    private void EndStrike()
    {
        _playerAnimator.SetBool("Strike", false);
        _isStriking = false;
    }

}

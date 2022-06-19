using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    Animator animator;

    public Move moveState;
    public Jump jumpState;
    public Dodge dodgeState;
    public Climb climbState;
    public Attack attackState;
    public Crouch crouchState;

    public GameObject Girl;
    public ParticleSystem scanEffect;

    PlayerTriggerler playerTriggerler;

    Vector3 lastSafePos;
    public Transform respawnPoint;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        playerTriggerler = GetComponentInChildren<PlayerTriggerler>();
    }

    protected override void InitState()
    {
        base.InitState();
        attackState.Init(this);
        moveState.Init(this);
        jumpState.Init(this);
        dodgeState.Init(this);
        climbState.Init(this);
        crouchState.Init(this);
    }

    protected override void Start()
    {
        base.Start();

        climbState.OnStateEnterDelegate += JumpCross;
        dodgeState.OnStateEnterDelegate += Dodge;
        footCollision.OnLandEvent.AddListener(OnLand);
    }

    public override void Update()
    {
        base.Update();

        if (currentState == CharacterState.Idle && Mathf.Abs(currentMovement) > 0.1f)
        {
            ChangeState(CharacterState.Move);
        }

        if (!IsOnGround())
        {
            ChangeState(CharacterState.Jump);
        }
        else
        {
            lastSafePos = (Vector2)transform.position - (characterRigidbody.velocity * .3f);
        }

        AddMovement(Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("VelocityY", characterRigidbody.velocity.y);
        animator.SetFloat("VelocityX", Mathf.Max(Mathf.Abs(currentMovement), Mathf.Abs(currentMoveSpeed)));

        if (Input.GetButtonDown("Jump"))
        {
            if(jumpState.DoJump())
            {
                animator.SetTrigger("Jump");
            }
        }

        if(Input.GetButtonDown("Dodge"))
        {
            ChangeState(CharacterState.Dash);
        }

        if(Input.GetButtonDown("Scan"))
        {
            Scan();
        }

        //if (Input.GetButtonDown("Fire1"))
        //{
        //    int attackCombo = attackState.DoAttack();
        //    if (attackCombo == 1)
        //    {
        //        animator.SetTrigger("Attack");
        //    }
        //    else if(attackCombo > 1)
        //    {
        //        Debug.Log("AttackNext : " + attackCombo);
        //        animator.SetTrigger("AttackNext");
        //    }
        //}

        if(currentState == CharacterState.Idle || currentState == CharacterState.Crouch)
        {
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                ChangeState(CharacterState.Crouch);
                if (Input.GetButtonDown("Jump"))
                {
                    if(crouchState.DoCrouch())
                    {
                        animator.SetTrigger("Jump");
                    }
                }
            }
            else
            {
                if(currentState == CharacterState.Crouch)
                {
                    ChangeState(CharacterState.Idle);
                }
            }
        }

        if (Input.GetButtonDown("Confirm"))
        {
            GameManager.Instance.ConfirmWord();
        }

        if(Input.GetButtonDown("Use"))
        {
            playerTriggerler.InputCollect();
        }
    }

    private void Scan()
    {
        Girl = GameManager.Instance.girl;
        ColliderDrawer[] platforms = Girl.GetComponentsInChildren<ColliderDrawer>();
        foreach (ColliderDrawer item in platforms)
        {
            item.ShowScanEffect();
        }
        scanEffect.Play();
    }

    protected void JumpUpdate()
    {
        if(characterRigidbody.velocity.y < 0)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }
        
    }

    protected void Dodge()
    {
        animator.SetTrigger("Dodge");
    }

    protected void OnLand()
    {
        animator.SetTrigger("Land");
    }

    protected void JumpCross()
    {
        animator.SetTrigger("JumpCross");
    }

    public void BackToSafePos()
    {
        transform.position = respawnPoint.position;
    }
}

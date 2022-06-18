using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Jump : StateBase
{
    public float bodyDownDistance = 1.0f;
    public float bodyDownYOffset = 1.0f;
    public float bodyDownXOffset = 1.0f;

    public float wallDetectDistance = 1.0f;
    public float wallDetectYOffset = 1.0f;
    public float wallDetectGap = 1.0f;

    [Range(0, 1)]
    public float airControl;

    public float wallJump = 100.0f;

    public JumpInfo[] jumpInfos;
    public JumpInfo wallJumpInfo;
    public float characterDropForce = 3.0f;

    public LayerMask layerMask;

    public RaycastHit2D hitInfoCache;

    private bool wallJumpReady;
    private int jumpTime = 0;
    private float wallJumpTimer;
    private float hitWallFaceDir;

    Climb climbStete = null;

    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Jump;
        base.Init(character);
        
    }

    public override void StateEnter()
    {

        if (owner.stateObjectList.ContainsKey(CharacterState.Climb))
        {
            climbStete = (Climb)owner.stateObjectList[CharacterState.Climb];
        }

        owner.moveSpeed *= airControl;

        if(jumpTime == 0)
        {
            jumpTime++;
        }

        base.StateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        JumpUpdate();
        JumpCrossUpdate();
    }

    public override void StateEnd()
    {
        base.StateEnd();
        jumpTime = 0;
        wallJumpTimer = 0.0f;
        wallJumpReady = false;
    }

    public bool DoJump()
    {
        if(!IsStateAviliable())
        {
            return false;
        }
        if (jumpInfos.Length == 0)
        {
            Debug.LogError("JumpInfos is empty");
            return false;
        }

        if(wallJumpReady)
        {
            float jumpForceX = 0.0f;
            if (jumpTime > 0)
            {
                Debug.Log("Wall Jump");
                owner.characterRigidbody.gravityScale = wallJumpInfo.jumpStartForce;
                owner.characterRigidbody.velocity = new Vector2(owner.characterRigidbody.velocity.x, 0.0f);

                jumpForceX = hitWallFaceDir * wallJump * -1;

                owner.characterRigidbody.AddForce(new Vector2(jumpForceX, Mathf.Sqrt(wallJumpInfo.jumpHeight * -2 * (Physics2D.gravity.y * wallJumpInfo.jumpStartForce))));
                return true;
            }
        }

        if (jumpInfos.Length > jumpTime)
        {
            owner.characterRigidbody.gravityScale = jumpInfos[jumpTime].jumpStartForce;
            owner.characterRigidbody.velocity = new Vector2(owner.characterRigidbody.velocity.x, 0.0f);

            owner.characterRigidbody.AddForce(new Vector2(0.0f, Mathf.Sqrt(jumpInfos[jumpTime].jumpHeight * -2 * (Physics2D.gravity.y * jumpInfos[jumpTime].jumpStartForce))));
            jumpTime++;
            return true;
        }
        return false;
    }

    public override bool IsStateAviliable()
    {
        if (owner.currentState == CharacterState.Climb || owner.currentState == CharacterState.Dash || owner.currentState == CharacterState.Attack || owner.currentState == CharacterState.Crouch)
        {
            return false;
        }
        return true;
    }

    private void JumpUpdate()
    {
        if (owner.characterRigidbody.velocity.y < 0)
        {
            owner.characterRigidbody.gravityScale = characterDropForce;
        }

        if(owner.IsOnGround())
        {
            owner.ChangeState(CharacterState.Idle);
        }

        if(jumpTime > 0 && owner.wallDetectHandler.hitWall && !wallJumpReady)
        {
            hitWallFaceDir = owner.faceDirection;
            wallJumpReady = true;
        }

        if(!owner.wallDetectHandler.hitWall && wallJumpReady)
        {
            wallJumpTimer += Time.deltaTime;
            if(wallJumpTimer > .2f)
            {
                wallJumpTimer = 0.0f;
                wallJumpReady = false;
            }
        }
    }

    private void JumpCrossUpdate()
    {
        int hitCount = 0;
        for (int idx = 0; idx < 6; idx++)
        {
            if (CreateCollisionRayCast(owner.transform.position + new Vector3(0, wallDetectGap * idx + wallDetectYOffset, 0), owner.transform.right * owner.faceDirection, wallDetectDistance))
            {
                hitCount++;
                Debug.DrawRay(owner.transform.position + new Vector3(0, wallDetectGap * idx + wallDetectYOffset, 0), owner.transform.right * owner.faceDirection * 1.0f, Color.red);

            }
            else
            {
                Debug.DrawRay(owner.transform.position + new Vector3(0, wallDetectGap * idx + wallDetectYOffset, 0), owner.transform.right * owner.faceDirection * 1.0f, Color.green);
                if (hitCount > 0)
                {
#if UNITY_EDITOR
                    owner.debugHelper[0].RayType = Kit.Physic.RaycastHelper.eRayType.Raycast;
                    owner.debugHelper[0].m_Direction = Vector2.up * -1;
                    owner.debugHelper[0].m_Distance = bodyDownDistance;
                    owner.debugHelper[0].m_LocalPosition = new Vector3(bodyDownXOffset, wallDetectGap * idx + wallDetectYOffset, 0);
                    owner.debugHelper[0].CheckPhysic();
#endif
                    Debug.DrawRay((owner.transform.position + new Vector3(bodyDownXOffset * owner.faceDirection, wallDetectGap * idx + wallDetectYOffset, 0)), owner.transform.up * -1.0f, Color.blue);
                    if (CreateCollisionRayCast((owner.transform.position + new Vector3(bodyDownXOffset * owner.faceDirection, wallDetectGap * idx + wallDetectYOffset, 0)), owner.transform.up * -1.0f, bodyDownDistance))
                    {
#if UNITY_EDITOR
                        owner.debugHelper[1].RayType = Kit.Physic.RaycastHelper.eRayType.SphereCast;
                        owner.debugHelper[1].m_Distance = owner.bodyCollision.bounds.size.y - owner.bodyCollision.bounds.size.x;
                        owner.debugHelper[1].m_LocalPosition = new Vector2(((hitInfoCache.point.x - owner.transform.position.x) * owner.faceDirection), hitInfoCache.point.y + owner.bodyCollision.bounds.size.x / 2 + bodyDownYOffset - owner.transform.position.y);
                        owner.debugHelper[1].m_Radius = owner.bodyCollision.bounds.size.x / 2;
                        owner.debugHelper[1].m_Direction = Vector2.up;
                        owner.debugHelper[1].CheckPhysic();
#endif

                        RaycastHit2D obstacleCast = Physics2D.CircleCast(new Vector2(hitInfoCache.point.x, hitInfoCache.point.y + owner.bodyCollision.bounds.size.x / 2 + bodyDownYOffset), owner.bodyCollision.bounds.size.x / 2, Vector2.up, owner.bodyCollision.bounds.size.y - owner.bodyCollision.bounds.size.x, layerMask);
                        if (!obstacleCast)
                        {
                            climbStete.ResetClimbInfo(hitInfoCache.point);
                            owner.ChangeState(CharacterState.Climb);
                        }
                    }
                    break;
                }

            }
        }
    }

    bool CreateCollisionRayCast(Vector2 startPos, Vector2 direction, float distance)
    {
        hitInfoCache = Physics2D.Raycast(startPos, direction, distance, layerMask);
        if (hitInfoCache)
        {
            return true;
        }
        return false;
    }
}

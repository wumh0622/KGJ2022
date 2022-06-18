using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterState
{
    Idle,
    Move,
    Attack,
    Climb,
    Jump,
    Crouch,
    Dash,
    Damage,
    Dead
}

[System.Serializable]
public struct JumpInfo
{
    public float jumpHeight;
    public float jumpStartForce;
}

[System.Serializable]
public struct ObjectForceInfo
{
    public Vector2 currentForce;
    public float forceDropTime;
    public float forceDropTimer;
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterBase : MonoBehaviour, IDamageHandle
{
    //Component
    public Rigidbody2D characterRigidbody;
    public Collider2D bodyCollision;
    public CharacterWallDetectHandler wallDetectHandler;
    public CharacterFootCollision footCollision;

    //State instance
    [HideInInspector] public Idle idleState;

#if UNITY_EDITOR
    public Kit.Physic.RaycastHelper[] debugHelper;
#endif

    [Range(0, 1)]
    public float forceWeight = 1.0f;
    public float moveAccelerateSmooth = 3.0f;
    public float characterDropForce = 3.0f;
    public float wallDetectThickness = 2.0f;

    //runtime
    public StateBase currentStateObject { protected set; get; }
    public Dictionary<CharacterState, StateBase> stateObjectList = new Dictionary<CharacterState, StateBase>();
    public Dictionary<string, ObjectForceInfo> forceList = new Dictionary<string, ObjectForceInfo>();
    public bool NeedWallDetect = false;
    public CharacterState currentState;

    [HideInInspector] public float moveSpeed = 10.0f;
    protected float currentMovement;
    [HideInInspector] public float targetMoveSpeed;
    protected float currentMoveSpeed;
    [HideInInspector] public float faceDirection = 1.0f;
    public Vector2 moveVelocity;
    public float jumpX;
    private bool disableMovement;
    private bool overrideMoveUpdate;
    private Vector2 extraForce = Vector2.zero;

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        debugHelper = GetComponents<Kit.Physic.RaycastHelper>();
#endif
        characterRigidbody = GetComponent<Rigidbody2D>();
        bodyCollision = GetComponent<CapsuleCollider2D>();
        footCollision = GetComponentInChildren<CharacterFootCollision>();
        if(NeedWallDetect)
        {
            CreateWallDetectTrigger();
        }
        InitState();
    }

    protected virtual void InitState()
    {
        idleState.Init(this);
    }

    protected virtual void Start()
    {
        ChangeState(CharacterState.Idle);
    }

    public virtual void Update()
    {
        currentStateObject.StateUpdate();
        UpdateCharacterForce();
        MoveUpdate();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
    }

    public virtual void MoveUpdate()
    {
        if(overrideMoveUpdate)
        {
            currentMoveSpeed = stateObjectList[currentState].StateMoveUpdate();
        }

        if (disableMovement)
        {
            return;
        }

        if (Mathf.Abs(currentMovement) > 0.1)
        {
            targetMoveSpeed = Mathf.Abs(currentMovement) * moveSpeed;
        }
        else
        {
            targetMoveSpeed = 0.0f;
        }

        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetMoveSpeed, Time.deltaTime * moveAccelerateSmooth);
    }

    public void OverrideMoveUpdate(bool enableOverride)
    {
        overrideMoveUpdate = enableOverride;
    }

    public void AddMovement(float moveDirection)
    {
        currentMovement = moveDirection;

        if (disableMovement)
        {
            return;
        }

        if (Mathf.Abs(moveDirection) > 0)
        {
            faceDirection = moveDirection > 0 ? 1.0f : -1.0f;
        }

        if (faceDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (faceDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ForceUpdateFaceDirection()
    {
        if (Mathf.Abs(currentMovement) > 0)
        {
            faceDirection = currentMovement > 0 ? 1.0f : -1.0f;
        }

        if (faceDirection < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (faceDirection > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void DisableMovement(bool disable)
    {
        disableMovement = disable;
        if(disable)
        {
            currentMoveSpeed = 0.0f;
        }
    }

    public bool IsOnGround()
    {
        return footCollision.onGround;
    }

    void UpdateVelocity()
    {
        if(NeedWallDetect)
        {
            if (wallDetectHandler.hitWall)
            {
                currentMoveSpeed = 0.0f;
            }
        }

        characterRigidbody.velocity = new Vector2(Mathf.Clamp(characterRigidbody.velocity.x, -20, 20), Mathf.Clamp(characterRigidbody.velocity.y, -20, 20));

        if (characterRigidbody.velocity.x == float.NaN || characterRigidbody.velocity.y == float.NaN)
        {
            characterRigidbody.velocity = Vector2.zero;
        }

        characterRigidbody.velocity = new Vector2((currentMoveSpeed * faceDirection), characterRigidbody.velocity.y);
    }

    void CreateWallDetectTrigger()
    {
        GameObject newWallDetectObject = Instantiate(new GameObject(), this.transform);
        newWallDetectObject.transform.position = bodyCollision.bounds.center + new Vector3(bodyCollision.bounds.extents.x, 0, 0);
        newWallDetectObject.name = "WallDetectObjectRight";
        newWallDetectObject.layer = 6;
        wallDetectHandler = newWallDetectObject.AddComponent<CharacterWallDetectHandler>();

        BoxCollider2D newCollider = newWallDetectObject.AddComponent<BoxCollider2D>();
        newCollider.isTrigger = true;
        newCollider.offset = new Vector2(wallDetectThickness / 2, 0);
        newCollider.size = new Vector2(wallDetectThickness, bodyCollision.bounds.size.y * .8f);
    }

    public virtual bool ChangeState(CharacterState state)
    {
        if(currentState == state && currentStateObject != null)
        {
            return true;
        }
        if(stateObjectList.ContainsKey(state))
        {
            if (stateObjectList[state].IsStateAviliable())
            {
                if (currentStateObject != null)
                {
                    currentStateObject.StateEnd();
                }

                currentState = stateObjectList[state].stateType;
                currentStateObject = stateObjectList[state];
                currentStateObject.StateEnter();
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;

    }

    public void AddForceToActor(ref ObjectForceInfo forceInfo, string forceName)
    {
        if (forceList.ContainsKey(forceName))
        {
            forceList.Remove(forceName);
        }
        forceList.Add(forceName, forceInfo);
    }

    private void UpdateCharacterForce()
    {
        if (forceList.Count == 0)
        {
            return;
        }
        Dictionary<string, ObjectForceInfo> newForceList = new Dictionary<string, ObjectForceInfo>();

        foreach (string key in forceList.Keys)
        {
            ObjectForceInfo newInfo = forceList[key];
            newInfo.forceDropTimer += Time.deltaTime;
            newInfo.currentForce *= 1 - newInfo.forceDropTimer / newInfo.forceDropTime;
            if (newInfo.forceDropTimer < newInfo.forceDropTime)
            {
                newForceList.Add(key, newInfo);
            }

        }
        forceList = newForceList;

        extraForce = Vector2.zero;
        if (forceList.Count > 0)
        {
            foreach (ObjectForceInfo info in forceList.Values)
            {
                extraForce += info.currentForce;
            }
        }

        extraForce *= forceWeight;
    }

    void IDamageHandle.ApplyDamage(AttackInfo info)
    {
        ObjectForceInfo forceInfo = new ObjectForceInfo();
        forceInfo.currentForce = new Vector2(5, 0);
        forceInfo.forceDropTime = 0.5f;
        AddForceToActor(ref forceInfo, "Damage");
        Debug.Log("Damage");
    }
}

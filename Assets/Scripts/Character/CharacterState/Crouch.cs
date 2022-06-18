using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crouch : StateBase
{
    public Vector3 cameraOffset;

    public LayerMask layerMask;
    public JumpInfo jumpInfo;
    public float crouchDetectGap = 0.5f;
    public float crouchDetectYOffset = 0.3f;
    public float crouchDetectDistance = 0.1f;

    RaycastHit2D hitInfoCache;
    public override void Init(CharacterBase character)
    {
        stateType = CharacterState.Crouch;
        base.Init(character);
    }

    public override void StateEnter()
    {
        base.StateEnter();
        owner.DisableMovement(true);
        //CameraManager.instance.SetCameraOffset(cameraOffset);

    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        owner.AddMovement(owner.faceDirection);

    }

    public override void StateEnd()
    {
        base.StateEnd();
        owner.DisableMovement(false);
        //CameraManager.instance.ResetCameraOffset();
    }

    public bool DoCrouch()
    {
        bool result = false;
        GameObject hitObject = null;
        for (int idx = 0; idx < 6; idx++)
        {
            if (!CreateCollisionRayCast(owner.transform.position - new Vector3(0, crouchDetectGap * idx + crouchDetectYOffset, 0), owner.transform.right * owner.faceDirection, crouchDetectDistance))
            {
                result = true;
                if(hitObject != null)
                {
                    PlatformObject platform = hitObject.GetComponent<PlatformObject>();
                    if (platform)
                    {
                        owner.characterRigidbody.AddForce(new Vector2(0.0f, Mathf.Sqrt(jumpInfo.jumpHeight * -2 * (Physics2D.gravity.y * jumpInfo.jumpStartForce))));
                        platform.ThroughPlatform();
                    }
                    break;
                }
            }
            else
            {
                hitObject = hitInfoCache.collider.gameObject;
            }
        }
        return result;
    }

    bool CreateCollisionRayCast(Vector2 startPos, Vector2 direction, float distance)
    {
        Debug.DrawRay(startPos, direction * distance, Color.red, 10.0f);
        hitInfoCache = Physics2D.Raycast(startPos, direction, distance, layerMask);
        if (hitInfoCache)
        {
            return true;
        }
        return false;
    }
}

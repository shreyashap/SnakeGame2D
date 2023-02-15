using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private Vector2 dPosition;
    public BodyPart following = null;
    private SpriteRenderer spriteRenderer;
    private const int PARTSREMEMBERED = 10;
    public Vector3[] previousPositions = new Vector3[PARTSREMEMBERED];
    private int setIndex = 0;
    private int getIndex = -(PARTSREMEMBERED - 1);
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetPosition()
    {
        setIndex = 0;
        getIndex = -(PARTSREMEMBERED - 1);
}
    public virtual void Update()
    {
        if (!GameController.instance.isAlive) return;
        Vector3 followPosition;

        if(following != null)
        {
            if(getIndex > -1)
            {
                followPosition = following.previousPositions[following.getIndex];
            }
            else
            {
                followPosition = following.transform.position;
            }
        }
        else
        {
            followPosition = gameObject.transform.position;
        }

        previousPositions[setIndex].x = gameObject.transform.position.x;
        previousPositions[setIndex].y = gameObject.transform.position.y;
        previousPositions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if (setIndex >= PARTSREMEMBERED)
            setIndex = 0;
        getIndex++;
        if (getIndex >= PARTSREMEMBERED)
            getIndex = 0;

        if(following != null)
        {
            Vector3 newPosition;
            if(getIndex > -1)
            {
                newPosition = followPosition;
            }
            else
            {
                newPosition = following.transform.position;
            }

            newPosition.z += 0.01f;
            SetMovement(newPosition - gameObject.transform.position);
            UpdateDirection();
            UpdatePosition();
        }
    }
    protected void SetMovement(Vector2 movement)
    {
        dPosition = movement;
    }

    protected void UpdatePosition()
    {
        gameObject.transform.position += (Vector3)dPosition;
    }

    protected void UpdateDirection()
    {
        if(dPosition.y > 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if(dPosition.y < 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        else if(dPosition.x > 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else if(dPosition.x < 0)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0,90);
        }
    }

    public void TurnIntoTail()
    {
        spriteRenderer.sprite = GameController.instance.tailSprite;
    }

    public void TurnIntoBodyPart()
    {
        spriteRenderer.sprite = GameController.instance.bodySprite;
    }
}

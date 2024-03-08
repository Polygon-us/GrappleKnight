using System;
using UnityEngine;

public class CollisionEvents
{
    
    private Action<Collision2D> _onCollisionStay;
    
    
    
    private Action<Collider2D> _onTriggerStay;
    private Action<Collider2D> _onTriggerExit;
   

    public void SubscribeCollisionEnter(Action<Collision2D> other)
    {
        _onCollisionStay += other;
    }

    public void UnsubscribeCollisionEnter(Action<Collision2D> other)
    {
        _onCollisionStay -= other;
    } 
    
    public void SubscribeTriggerStay(Action<Collider2D> other)
    {
        _onTriggerStay += other;
    }

    public void UnsubscribeTriggerStay(Action<Collider2D> other)
    {
        _onTriggerStay -= other;
    }
    
    public void SubscribeTriggerExit(Action<Collider2D> other)
    {
        _onTriggerExit += other;
    }

    public void UnsubscribeTriggerExit(Action<Collider2D> other)
    {
        _onTriggerExit -= other;
    }
    
    
    public void CollisionStayDispatch(Collision2D other)
    {
        _onCollisionStay?.Invoke(other);
    }
    
    public void TriggerStayDispatch(Collider2D other)
    {
        _onTriggerStay?.Invoke(other);
    } 
    public void TriggerExitDispatch(Collider2D other)
    {
        _onTriggerExit?.Invoke(other);
    }
    
    
}
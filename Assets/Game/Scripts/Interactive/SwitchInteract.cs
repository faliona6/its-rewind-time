using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

public class SwitchInteract : MonoBehaviour, IInteractable
{
    Animator animator;
    [SerializeField] UnityEvent switchOnEvent;
    [SerializeField] UnityEvent switchOffEvent;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        animator.SetBool("switchOn", !animator.GetBool("switchOn"));
        var action = animator.GetBool("switchOn") ? switchOnEvent : switchOffEvent;
        if (action != null)
            action.Invoke();
    }
}

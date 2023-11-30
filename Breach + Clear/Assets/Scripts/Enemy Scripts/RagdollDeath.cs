using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDeath : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Animator animator = null;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColiders;
    
        // Start is called before the first frame update
    private void Start()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColiders = GetComponentsInChildren<Collider>();

        ToggleRagdoll(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Death()
    {
        Debug.Log("Dead!");
        ToggleRagdoll(true);
    }

    private void ToggleRagdoll(bool state)
    {
        animator.enabled = !state;
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider collider in ragdollColiders)
        {

        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInteract : MonoBehaviour, IInteractable
{
    public GameObject intro;
    public GameObject house;
    
    //teleport setup stuff
    [SerializeField]
    public Transform destination;

    [SerializeField]
    private PlayerMovement player;

    public void Interact()
    {
        player.Teleport(destination.position);
        intro.SetActive(false);
        house.SetActive(true);
    }

    //visualizing teleport destination
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(destination.position, .4f);
        var direction = destination.TransformDirection(Vector3.forward);
        Gizmos.DrawRay(destination.position, direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of nodes that the AI can walk to from this node")]
    private GameObject[] adjacentByFoot;

    [SerializeField]
    [Tooltip("List of nodes that the AI can jump to from this node")]
    private GameObject[] adjacentByJump;

    [SerializeField]
    [Tooltip("List of nodes that the AI can fly to from this node")]
    private GameObject[] adjacentByFlight;

    public GameObject[] GetAdjacentByFoot()
    {
        return adjacentByFoot;
    }

    public GameObject[] GetAdjacentByJump()
    {
        return adjacentByJump;
    }

    public GameObject[] GetAdjacentByFlight()
    {
        return adjacentByFlight;
    }
}

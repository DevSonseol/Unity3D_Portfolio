using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RoamingObject : MonoBehaviour
{
    public Vector3[] roamingPoints;
    [SerializeField] private NavMeshAgent nma;
    [SerializeField] private Character character;
    [SerializeField] private bool isRoaming = true;
    [SerializeField] private float gizmoRadius = 1;
    [SerializeField] private int countPoint;

    void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        character = GetComponent<Character>();
    }

    void Start()
    {

        countPoint = 0;

        if (roamingPoints.Length <= 1)
            isRoaming = false;
        else
            nma.SetDestination(roamingPoints[0]);


        if (character != null)
            character.ChangeState(CharacterState.Move);
    }

    void Update()
    {
        if (!isRoaming) return;

        if (nma.velocity.sqrMagnitude > 0.04f && nma.remainingDistance < 0.3f)
        {
            countPoint++;
            if(countPoint >= roamingPoints.Length)
                countPoint = 0;

            nma.ResetPath();
            nma.SetDestination(roamingPoints[countPoint]);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < roamingPoints.Length; ++i)
        {
            Gizmos.DrawSphere(roamingPoints[i], gizmoRadius);
        }
    }
}

using System.Collections;
using UnityEngine;

public class wilk : MonoBehaviour
{
    public Transform targetTransform;
    private NavMeshAgent agent;
    private Transform myTransform;
    private float radius = 100;
    private LayerMask raycastLayer;

    private IEnumerator DoCheck()
    {
        for (;;)
        {
            SearchForTarget();
            MoveToTarget();
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //SearchForTarget();
        //MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (targetTransform != null)
        {
            SetNavDestination(targetTransform);
        }
    }

    private void SearchForTarget()
    {
        if (targetTransform == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);

            if (hitColliders.Length > 0)
            {
                int randomint = Random.Range(0, hitColliders.Length);
                targetTransform = hitColliders[randomint].transform;
            }
        }

        if (targetTransform != null && targetTransform.GetComponent<CapsuleCollider>().enabled == false)
        {
            targetTransform = null;
        }
    }

    private void SetNavDestination(Transform dest)
    {
        agent.SetDestination(dest.position);
    }

    // Use this for initialization
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer("Player");

        StartCoroutine(DoCheck());
    }
}
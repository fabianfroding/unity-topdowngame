using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView: MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask playerMask;
    public LayerMask environmentMask;

    [HideInInspector]
    public List<Transform> visiblePlayers = new List<Transform>();

    public Vector2 GetDirectionFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angle -= transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private void Start()
    {
        StartCoroutine("FindPlayerWithDelay", .2f);
    }

    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisiblePlayer();
        }
    }

    public GameObject FindVisiblePlayer()
    {
        visiblePlayers.Clear();
        Collider2D[] playerInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInViewRadius.Length; i++)
        {
            Transform player = playerInViewRadius[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.up, dirToPlayer) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, player.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, distToTarget, environmentMask);
                if (!hit)
                {
                    visiblePlayers.Add(player);
                    return player.gameObject;
                }
                else
                {
                    return hit.collider.gameObject;
                }
            }
        }

        return null;
    }
}

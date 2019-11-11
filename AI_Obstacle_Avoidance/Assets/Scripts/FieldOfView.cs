using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
  [Range(0, 360)]
  public float viewAngle;
  public float viewRadius;

  public LayerMask targetMask, obstacleMask;

  [HideInInspector]
  public List<Transform> visibleTargets = new List<Transform>();

  private void Start()
  {
    StartCoroutine("FindTargetsWithDelay", 0.2f);
  }

  IEnumerator FindTargetsWithDelay(float delay)
  {
    while(true)
    {
      yield return new WaitForSeconds(delay);
      FindVisibleTargets();
    }
  }

  void FindVisibleTargets()
  {
    visibleTargets.Clear();
    Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

    for(int i = 0; i < targetsInViewRadius.Length; ++i)
    {
      Transform target = targetsInViewRadius[i].transform;
      Vector3 dirToTarget = (target.position - transform.position).normalized;
      Debug.Log("Angle: " + Vector3.Angle(transform.right, dirToTarget));
      if(Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2)
      {
        Debug.Log("Within Range");
        float distToTarget = Vector3.Distance(transform.position, target.position);

        if(!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
        {
          visibleTargets.Add(target);
        }
      }
    }
  }

  public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
  {
    if (!angleIsGlobal)
      angleInDeg += transform.eulerAngles.z;

    return new Vector3(
        Mathf.Cos(angleInDeg * Mathf.Deg2Rad),
        Mathf.Sin(angleInDeg * Mathf.Deg2Rad),
        0);
  }
}

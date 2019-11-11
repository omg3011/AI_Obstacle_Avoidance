using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  //-- WP
  public List<Transform> list_WP_Transform;
  public float _wp_reached_threshold = 1.0f;
  int currWPIndex = 0;


  //-- Reference
  Transform target;

  //-- Twerkable
  public float speed = 5;
  float dodgeWeight = 3;
  float detection_range = 1;
  float fov_angle = 90;
  
  //-- Private
  Vector3 moveDir;
  Vector3 originalDir;

  //-- Camera
  Camera cam;

  private void Start()
  {
    cam = Camera.main;
    InitWP();
  }

  /////////////////////////////////////////////////////////////////////////////////////////////
  //
  //  Waypoints
  //
  /////////////////////////////////////////////////////////////////////////////////////////////
  void InitWP()
  {
    transform.position = list_WP_Transform[0].position;
    moveDir = (list_WP_Transform[currWPIndex+1].position - list_WP_Transform[currWPIndex].position).normalized;
    originalDir = moveDir;
  }

  void Update_WP_Dir()
  {
    //-- Reach WP
    if(Vector3.Distance(transform.position, list_WP_Transform[currWPIndex].position) < _wp_reached_threshold)
    {
      // Go to next WP + Clamp WP
      currWPIndex++;
      if (currWPIndex >= list_WP_Transform.Count)
        currWPIndex = 0;

      // Update new dir
      moveDir = (list_WP_Transform[currWPIndex].position - transform.position).normalized;
    }
  }

  void Test2()
  {
      Vector3 headingVector = transform.position - target.position;
      //Vector3 headingVector = target.position - transform.position;
      float len = headingVector.magnitude;

      if(len < detection_range)
      {
          Vector3 adj = headingVector.normalized;
          float diff = (detection_range - len) * dodgeWeight;
          adj *= diff;
          moveDir -= adj;
          moveDir.Normalize();
      }
  }

  bool IsInCone()
  {
      Vector3 dir = (target.position - transform.position).normalized;
      float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


      Debug.Log("Angle: " + angle + ", Left: " + (transform.localEulerAngles.x - fov_angle / 2));
      if(angle > transform.rotation.x - fov_angle/2)
      {
          if (angle < transform.localEulerAngles.x + fov_angle / 2)
              return true;
      }

      return false;
  }

  /////////////////////////////////////////////////////////////////////////////////////////////
  //
  //  Helper Functions
  //
  /////////////////////////////////////////////////////////////////////////////////////////////
  public static float Angle(Vector2 p_vector2)
  {
    if (p_vector2.x < 0)
    {
      return 360 - (Mathf.Atan2(p_vector2.y, p_vector2.x) * Mathf.Rad2Deg * -1);
    }
    else
    {
      return Mathf.Atan2(p_vector2.y, p_vector2.x) * Mathf.Rad2Deg;
    }
  }


  /////////////////////////////////////////////////////////////////////////////////////////////
  //
  //  Rotate Varieties
  //
  /////////////////////////////////////////////////////////////////////////////////////////////
  void UpdateRotate_By_MoveDir()
  {
    float angle = Angle(moveDir);
    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
  }
  void UpdateRotate_By_MousePos()
  {
    Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));


    Vector3 dir = (mousePos - transform.position).normalized;
    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Angle(dir));

  }

  private void Update()
  {
    //-- Update: WP dir
    Update_WP_Dir();


    //-- Update: Move & Rotate
    UpdateRotate_By_MousePos();
    transform.position += moveDir * speed * Time.deltaTime;
  }
}

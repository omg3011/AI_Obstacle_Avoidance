using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
  //-- WP
  public List<Transform> list_WP_Transform;
  public float _wp_reached_threshold = 1.0f;
  int currWPIndex = 0;

  //-- Speed
  public float speed = 5.0f;
  
  //-- Private
  Vector3 moveDir;

  //-- Camera
  Camera cam;

  void Start()
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
    moveDir = (list_WP_Transform[currWPIndex + 1].position - list_WP_Transform[currWPIndex].position).normalized;
  }

  void Update_WP_Dir()
  {
    //-- Reach WP
    if (Vector3.Distance(transform.position, list_WP_Transform[currWPIndex].position) < _wp_reached_threshold)
    {
      // Go to next WP + Clamp WP
      currWPIndex++;
      if (currWPIndex >= list_WP_Transform.Count)
        currWPIndex = 0;

      // Update new dir
      moveDir = (list_WP_Transform[currWPIndex].position - transform.position).normalized;
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

  // Update is called once per frame
  void Update()
  {
    // Update: WP
    Update_WP_Dir();

    // Update: Movement and Rotation
    UpdateRotate_By_MoveDir();
    //UpdateRotate_By_MousePos();
    transform.position += moveDir * speed * Time.deltaTime;
  }
}

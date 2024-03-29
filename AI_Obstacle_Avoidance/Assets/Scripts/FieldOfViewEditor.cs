﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
  private void OnSceneGUI()
  {
    FieldOfView fov = (FieldOfView)target;
    Handles.color = Color.white;
    Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);
    Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
    Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

    Handles.color = Color.red;
    foreach (Transform visibleTaraget in fov.visibleTargets)
    {
      Handles.DrawLine(fov.transform.position, visibleTaraget.position);
    }
  }
}

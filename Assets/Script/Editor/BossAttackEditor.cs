using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossAttack))]
public class BossAttackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BossAttack bossAttack = (BossAttack)target;

        // Display a custom button in the Inspector
        if (GUILayout.Button("Calculate Rotation"))
        {
            // Code to be executed when the button is clicked
            bossAttack.CalculateRotation();
        }
    }
}

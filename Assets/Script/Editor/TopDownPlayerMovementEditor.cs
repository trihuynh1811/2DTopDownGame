using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopDownPlayerMovement))]
public class YourScriptEditor : Editor
{
    SerializedProperty cam;
    SerializedProperty camFollowPos;
    SerializedProperty mainCam;
    SerializedProperty offset;
    SerializedProperty pickUp;
    SerializedProperty pickUpMask;
    SerializedProperty pickUpRadius;
    SerializedProperty boxCollider;
    SerializedProperty gunPos;
    SerializedProperty buffPos;
    SerializedProperty animator;
    SerializedProperty rb;
    SerializedProperty speed;
    SerializedProperty magnitude;

    SerializedProperty healthSlider;
    SerializedProperty armourSlider;
    SerializedProperty energySlider;
    SerializedProperty maxHealth;
    SerializedProperty maxArmour;
    SerializedProperty maxEnergy;
    SerializedProperty regenerateArmourRate;
    SerializedProperty regenerateEnegeyRate;
    SerializedProperty weaponStatCanvas;
    SerializedProperty weaponDmg;
    SerializedProperty weaponRoF;
    SerializedProperty weaponEnergyConsume;
    SerializedProperty weaponCriticalHit;
    SerializedProperty weaponAccuracy;
    SerializedProperty healthText;
    SerializedProperty armourText;
    SerializedProperty energyText;

    bool showGeneralSetting, showUiSetting = false;

    private void OnEnable()
    {
        cam = serializedObject.FindProperty("cam");
        camFollowPos = serializedObject.FindProperty("camFollowPos");
        mainCam = serializedObject.FindProperty("mainCam");
        offset = serializedObject.FindProperty("offset");
        pickUp = serializedObject.FindProperty("pickUp");
        pickUpMask = serializedObject.FindProperty("pickUpMask");
        pickUpRadius = serializedObject.FindProperty("pickUpRadius");
        boxCollider = serializedObject.FindProperty("boxCollider");
        gunPos = serializedObject.FindProperty("gunPos");
        buffPos = serializedObject.FindProperty("buffPos");
        animator = serializedObject.FindProperty("animator");
        rb = serializedObject.FindProperty("rb");
        speed = serializedObject.FindProperty("speed");
        magnitude = serializedObject.FindProperty("magnitude");

        healthSlider = serializedObject.FindProperty("healthSlider");
        armourSlider = serializedObject.FindProperty("armourSlider");
        energySlider = serializedObject.FindProperty("energySlider");
        maxHealth = serializedObject.FindProperty("maxHealth");
        maxArmour = serializedObject.FindProperty("maxArmour");
        maxEnergy = serializedObject.FindProperty("maxEnergy");
        regenerateArmourRate = serializedObject.FindProperty("regenerateArmourRate");
        regenerateEnegeyRate = serializedObject.FindProperty("regenerateEnegeyRate");
        weaponStatCanvas = serializedObject.FindProperty("weaponStatCanvas");
        weaponDmg = serializedObject.FindProperty("weaponDmg");
        weaponRoF = serializedObject.FindProperty("weaponRoF");
        weaponEnergyConsume = serializedObject.FindProperty("weaponEnergyConsume");
        weaponCriticalHit = serializedObject.FindProperty("weaponCriticalHit");
        weaponAccuracy = serializedObject.FindProperty("weaponAccuracy");
        healthText = serializedObject.FindProperty("healthText");
        armourText = serializedObject.FindProperty("armourText");
        energyText = serializedObject.FindProperty("energyText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showGeneralSetting = EditorGUILayout.BeginFoldoutHeaderGroup(showGeneralSetting, "General Setting");
        if (showGeneralSetting)
        {
            ShowGeneralSetting();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showUiSetting = EditorGUILayout.BeginFoldoutHeaderGroup(showUiSetting, "UI Setting");
        if (showUiSetting)
        {
            ShowUiSetting();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }

    void ShowGeneralSetting()
    {
        EditorGUILayout.PropertyField(cam);
        EditorGUILayout.PropertyField(camFollowPos);
        EditorGUILayout.PropertyField(mainCam);
        EditorGUILayout.PropertyField(offset);
        EditorGUILayout.PropertyField(pickUp);
        EditorGUILayout.PropertyField(pickUpMask);
        EditorGUILayout.PropertyField(pickUpRadius);
        EditorGUILayout.PropertyField(boxCollider);
        EditorGUILayout.PropertyField(gunPos);
        EditorGUILayout.PropertyField(buffPos);
        EditorGUILayout.PropertyField(animator);
        EditorGUILayout.PropertyField(rb);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(magnitude);
    }

    void ShowUiSetting()
    {
        EditorGUILayout.PropertyField(healthSlider);
        EditorGUILayout.PropertyField(armourSlider);
        EditorGUILayout.PropertyField(energySlider);
        EditorGUILayout.PropertyField(maxHealth);
        EditorGUILayout.PropertyField(maxArmour);
        EditorGUILayout.PropertyField(maxEnergy);
        EditorGUILayout.PropertyField(regenerateArmourRate);
        EditorGUILayout.PropertyField(regenerateEnegeyRate);
        EditorGUILayout.PropertyField(weaponStatCanvas);
        EditorGUILayout.PropertyField(weaponDmg);
        EditorGUILayout.PropertyField(weaponRoF);
        EditorGUILayout.PropertyField(weaponEnergyConsume);
        EditorGUILayout.PropertyField(weaponCriticalHit);
        EditorGUILayout.PropertyField(weaponAccuracy);
        EditorGUILayout.PropertyField(healthText);
        EditorGUILayout.PropertyField(armourText);
        EditorGUILayout.PropertyField(energyText);
    }
}

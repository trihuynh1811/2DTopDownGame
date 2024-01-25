#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{
    SerializedProperty gunType;

    SerializedProperty laser;
    SerializedProperty laserStart, laserEnd;

    SerializedProperty flameParticle;

    SerializedProperty hitEnemyLayerMask;
    SerializedProperty hitObjectLayerMask;
    SerializedProperty rayCastLength;

    SerializedProperty damage;
    SerializedProperty energyConsume;
    SerializedProperty gunSprite;
    SerializedProperty firePoint;
    SerializedProperty bullet;
    SerializedProperty maxBulletSpeed;
    SerializedProperty minBulletSpeed;
    SerializedProperty fireRate;
    SerializedProperty spreadAngle;
    SerializedProperty numberOfBullet;

    void OnEnable()
    {
        gunType = serializedObject.FindProperty("gunType");

        laser = serializedObject.FindProperty("laser");
        laserStart = serializedObject.FindProperty("laserStart");
        laserEnd = serializedObject.FindProperty("laserEnd");

        flameParticle = serializedObject.FindProperty("flameParticle");

        hitEnemyLayerMask = serializedObject.FindProperty("hitEnemyLayerMask");
        hitObjectLayerMask = serializedObject.FindProperty("hitObjectLayerMask");
        rayCastLength = serializedObject.FindProperty("rayCastLength");

        damage = serializedObject.FindProperty("damage");
        energyConsume = serializedObject.FindProperty("energyConsume");
        gunSprite = serializedObject.FindProperty("gunSprite");
        firePoint = serializedObject.FindProperty("firePoint");
        bullet = serializedObject.FindProperty("bullet");
        maxBulletSpeed = serializedObject.FindProperty("maxBulletSpeed");
        minBulletSpeed = serializedObject.FindProperty("minBulletSpeed");
        fireRate = serializedObject.FindProperty("fireRate");
        spreadAngle = serializedObject.FindProperty("spreadAngle");
        numberOfBullet = serializedObject.FindProperty("numberOfBullet");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(gunType);
        displayGeneralSetting();

        switch ((Gun.GunType)gunType.enumValueIndex)
        {
            case Gun.GunType.NormalGun:
                displaySettingForNormalGun();
                break;

            case Gun.GunType.Flamethrower:
                displaySettingForFlameThrower();
                break;

            case Gun.GunType.LaserGun:
                displaySettingForLaser();
                break;
        }

        // Add your GUI elements here using SerializedProperties

        serializedObject.ApplyModifiedProperties();
    }

    public void displayGeneralSetting()
    {
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(energyConsume);
        EditorGUILayout.PropertyField(gunSprite);
        EditorGUILayout.PropertyField(firePoint);
        EditorGUILayout.PropertyField(fireRate);
    }

    public void displaySettingForNormalGun()
    {
        EditorGUILayout.PropertyField(bullet);
        EditorGUILayout.PropertyField(maxBulletSpeed);
        EditorGUILayout.PropertyField(minBulletSpeed);
        EditorGUILayout.PropertyField(spreadAngle);
        EditorGUILayout.PropertyField(numberOfBullet);
    }

    public void displaySettingForFlameThrower()
    {
        EditorGUILayout.PropertyField(flameParticle);
        EditorGUILayout.PropertyField(hitEnemyLayerMask);
        EditorGUILayout.PropertyField(rayCastLength);
    }


    public void displaySettingForLaser()
    {
        EditorGUILayout.PropertyField(laser);
        EditorGUILayout.PropertyField(hitEnemyLayerMask);
        EditorGUILayout.PropertyField(hitObjectLayerMask);
        EditorGUILayout.PropertyField(rayCastLength);
        EditorGUILayout.PropertyField(laserStart);
        EditorGUILayout.PropertyField(laserEnd);
    }

}
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAttack))]
public class EnemyAttackEditor : Editor
{
    SerializedProperty usageType;
    SerializedProperty attackType;
    SerializedProperty useDeadEffect;
    SerializedProperty deathEffect;
    SerializedProperty useAttackAnimation;
    SerializedProperty animator;
    SerializedProperty deathClip;
    SerializedProperty attackClip;
    SerializedProperty explosionDmg;
    SerializedProperty explosionForce;
    SerializedProperty explosionTime;
    SerializedProperty splashRadius;

    SerializedProperty haveMultipleFirePoint;
    SerializedProperty firePointList;
    SerializedProperty firePoint;
    SerializedProperty fireRate;
    SerializedProperty damage;

    SerializedProperty flameParticleList;
    SerializedProperty flameRaycastLength;
    SerializedProperty gunTransform;
    SerializedProperty flameDetectPlayerList;
    SerializedProperty gunRotationSpeed;

    SerializedProperty laser;
    SerializedProperty laserLength;
    SerializedProperty laserExistTime;
    SerializedProperty laserHitMask;
    SerializedProperty laserEnd;

    SerializedProperty bullet;
    SerializedProperty numberOfBullet;
    SerializedProperty bulletSpeed;
    SerializedProperty spreadAngle, minBulletSpeed, maxBulletSpeed;

    private void OnEnable()
    {
        usageType = serializedObject.FindProperty("usageType");
        attackType = serializedObject.FindProperty("attackType");
        useDeadEffect = serializedObject.FindProperty("useDeadEffect");
        deathEffect = serializedObject.FindProperty("deathEffect");
        useAttackAnimation = serializedObject.FindProperty("useAttackAnimation");
        animator = serializedObject.FindProperty("animator");
        deathClip = serializedObject.FindProperty("deathClip");
        attackClip = serializedObject.FindProperty("attackClip");
        explosionDmg = serializedObject.FindProperty("explosionDmg");
        explosionForce = serializedObject.FindProperty("explosionForce");
        explosionTime = serializedObject.FindProperty("explosionTime");
        splashRadius = serializedObject.FindProperty("splashRadius");

        haveMultipleFirePoint = serializedObject.FindProperty("haveMultipleFirePoint");
        firePointList = serializedObject.FindProperty("firePointList");
        firePoint = serializedObject.FindProperty("firePoint");
        fireRate = serializedObject.FindProperty("fireRate");
        damage = serializedObject.FindProperty("damage");

        flameParticleList = serializedObject.FindProperty("flameParticleList");
        flameRaycastLength = serializedObject.FindProperty("flameRaycastLength");
        gunTransform = serializedObject.FindProperty("gunTransform");
        flameDetectPlayerList = serializedObject.FindProperty("flameDetectPlayerList");
        gunRotationSpeed = serializedObject.FindProperty("gunRotationSpeed");

        laser = serializedObject.FindProperty("laser");
        laserLength = serializedObject.FindProperty("laserLength");
        laserExistTime = serializedObject.FindProperty("laserExistTime");
        laserHitMask = serializedObject.FindProperty("laserHitMask");
        laserEnd = serializedObject.FindProperty("laserEnd");

        bullet = serializedObject.FindProperty("bullet");
        numberOfBullet = serializedObject.FindProperty("numberOfBullet");
        bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        spreadAngle = serializedObject.FindProperty("spreadAngle");
        minBulletSpeed = serializedObject.FindProperty("minBulletSpeed");
        maxBulletSpeed = serializedObject.FindProperty("maxBulletSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(usageType);
        switch ((EnemyAttack.UsageType)usageType.enumValueIndex)
        {
            case EnemyAttack.UsageType.Normal:
                EditorGUILayout.PropertyField(attackType);

                switch ((EnemyAttack.AttackType)attackType.enumValueIndex)
                {
                    case EnemyAttack.AttackType.SelfDestruct:
                        DisplayDeathEffectSetting();
                        DisplayExplosionSetting();
                        break;
                    case EnemyAttack.AttackType.ShootLaser:
                        DisplayGeneralSetting();
                        DisplayPlayerMask();
                        DisplayLaserDroneSetting();
                        DisplayDeathEffectSetting();
                        break;
                    case EnemyAttack.AttackType.ShootFlame:
                        DisplayGeneralSetting();
                        //DisplayPlayerMask();
                        DisplayFlameThrowerSetting();
                        DisplayDeathEffectSetting();
                        break;
                    case EnemyAttack.AttackType.ShootProjectile:
                        DisplayGeneralSetting();
                        DisplayShotgunBotSetting();
                        DisplayDeathEffectSetting();
                        break;
                }
                break;
            case EnemyAttack.UsageType.AnimationFunction:
                break;
        }


        serializedObject.ApplyModifiedProperties();
    }

    void DisplayDeathEffectSetting()
    {
        EditorGUILayout.PropertyField(useDeadEffect);
        if (useDeadEffect.boolValue)
        {
            EditorGUILayout.PropertyField(deathEffect);
        }
        else
        {
            EditorGUILayout.PropertyField(animator);
            EditorGUILayout.PropertyField(deathClip);
        }
    }

    void DisplayGeneralSetting()
    {
        EditorGUILayout.PropertyField(haveMultipleFirePoint);
        if (haveMultipleFirePoint.boolValue)
        {
            EditorGUILayout.PropertyField(firePointList, true);
        }
        else
        {
            EditorGUILayout.PropertyField(firePoint);
        }
        EditorGUILayout.PropertyField(fireRate);
        EditorGUILayout.PropertyField(damage);

    }

    void DisplayLaserDroneSetting()
    {
        EditorGUILayout.PropertyField(laser);
        EditorGUILayout.PropertyField(laserLength);
        EditorGUILayout.PropertyField(laserExistTime);
        EditorGUILayout.PropertyField(laserEnd);
    }

    void DisplayPlayerMask()
    {
        EditorGUILayout.PropertyField(laserHitMask);
    }

    void DisplayExplosionSetting()
    {
        EditorGUILayout.PropertyField(explosionDmg);
        EditorGUILayout.PropertyField(explosionForce);
        EditorGUILayout.PropertyField(explosionTime);
        EditorGUILayout.PropertyField(splashRadius);
    }

    void DisplayFlameThrowerSetting()
    {
        EditorGUILayout.PropertyField(flameParticleList);
        EditorGUILayout.PropertyField(flameRaycastLength);
        EditorGUILayout.PropertyField(gunRotationSpeed);
        EditorGUILayout.PropertyField(flameDetectPlayerList);
        EditorGUILayout.PropertyField(gunTransform);
    }

    void DisplayShotgunBotSetting()
    {
        EditorGUILayout.PropertyField(bullet);
        EditorGUILayout.PropertyField(bulletSpeed);
        EditorGUILayout.PropertyField(spreadAngle);
        EditorGUILayout.PropertyField(numberOfBullet);
        EditorGUILayout.PropertyField(minBulletSpeed);
        EditorGUILayout.PropertyField(maxBulletSpeed);
        EditorGUILayout.PropertyField(useAttackAnimation);
        EditorGUILayout.PropertyField(animator);
        EditorGUILayout.PropertyField(attackClip);
    }
}
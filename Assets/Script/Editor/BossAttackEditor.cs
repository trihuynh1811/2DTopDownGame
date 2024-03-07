using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossAttack))]
public class BossAttackEditor : Editor
{
    SerializedProperty bossType;
    SerializedProperty enemyTakeDmg;
    SerializedProperty firePoint;
    SerializedProperty gunHolderPos;
    SerializedProperty bullet;
    SerializedProperty gatling;
    SerializedProperty flamethrower;
    SerializedProperty missleLauncher;
    SerializedProperty flameParticle;
    SerializedProperty flameDetectPlayer;
    SerializedProperty minXpos;
    SerializedProperty maxXpos;
    SerializedProperty maxYpos;
    SerializedProperty minYpos;
    SerializedProperty missleSpeed;
    SerializedProperty timeBtwLaunchingMissile;
    SerializedProperty maxNumberMissle;
    SerializedProperty minNumberMissle;
    SerializedProperty missleIndicator;
    SerializedProperty missle;
    SerializedProperty missleSpawnPoint;
    SerializedProperty bulletDamage;
    SerializedProperty flameDamage;
    SerializedProperty gunHolderRotateSpeed, fireRate, flameFireRate, spreadAngle, numberOfBullet, maxBulletSpeed, minBulletSpeed;
    SerializedProperty rotation;
    SerializedProperty numberOfShootPoint;
    SerializedProperty radiusMultiplier;
    SerializedProperty shootPointList;

    SerializedProperty animator;
    SerializedProperty attackClip;
    SerializedProperty ring;
    SerializedProperty drone;
    SerializedProperty laserLines;
    SerializedProperty lasers;
    SerializedProperty ringRotationSpeed;
    SerializedProperty minRotateTime;
    SerializedProperty maxRotateTime;
    SerializedProperty laserLength;
    SerializedProperty damageRate;
    SerializedProperty randomLaserTime;
    SerializedProperty shootFireBallRate;
    SerializedProperty laserDmg;
    SerializedProperty hitMask;

    SerializedProperty useEffectWhenDie;
    SerializedProperty deathEffect;
    SerializedProperty explosionDmg;
    SerializedProperty explosionForce;
    SerializedProperty explosionTime;
    SerializedProperty splashRadius;

    void OnEnable()
    {
        bossType = serializedObject.FindProperty("bossType");

        enemyTakeDmg = serializedObject.FindProperty("enemyTakeDmg");

        firePoint = serializedObject.FindProperty("firePoint");
        gunHolderPos = serializedObject.FindProperty("gunHolderPos");
        bullet = serializedObject.FindProperty("bullet");
        gatling = serializedObject.FindProperty("gatling");
        flamethrower = serializedObject.FindProperty("flamethrower");
        missleLauncher = serializedObject.FindProperty("missleLauncher");
        flameParticle = serializedObject.FindProperty("flameParticle");
        flameDetectPlayer = serializedObject.FindProperty("flameDetectPlayer");
        minXpos = serializedObject.FindProperty("minXpos");
        maxXpos = serializedObject.FindProperty("maxXpos");
        minYpos = serializedObject.FindProperty("minYpos");
        maxYpos = serializedObject.FindProperty("maxYpos");
        missleSpeed = serializedObject.FindProperty("missleSpeed");
        timeBtwLaunchingMissile = serializedObject.FindProperty("timeBtwLaunchingMissile");
        maxNumberMissle = serializedObject.FindProperty("maxNumberMissle");
        minNumberMissle = serializedObject.FindProperty("minNumberMissle");
        missleIndicator = serializedObject.FindProperty("missleIndicator");
        missle = serializedObject.FindProperty("missle");
        missleSpawnPoint = serializedObject.FindProperty("missleSpawnPoint");
        bulletDamage = serializedObject.FindProperty("bulletDamage");
        flameDamage = serializedObject.FindProperty("flameDamage");
        gunHolderRotateSpeed = serializedObject.FindProperty("gunHolderRotateSpeed");
        fireRate = serializedObject.FindProperty("fireRate");
        flameFireRate = serializedObject.FindProperty("flameFireRate");
        spreadAngle = serializedObject.FindProperty("spreadAngle");
        numberOfBullet = serializedObject.FindProperty("numberOfBullet");
        maxBulletSpeed = serializedObject.FindProperty("maxBulletSpeed");
        minBulletSpeed = serializedObject.FindProperty("minBulletSpeed");

        rotation = serializedObject.FindProperty("rotation");
        numberOfShootPoint = serializedObject.FindProperty("numberOfShootPoint");
        radiusMultiplier = serializedObject.FindProperty("radiusMultiplier");
        shootPointList = serializedObject.FindProperty("shootPointList");

        animator = serializedObject.FindProperty("animator");
        attackClip = serializedObject.FindProperty("attackClip");
        ring = serializedObject.FindProperty("ring");
        drone = serializedObject.FindProperty("drone");
        laserLines = serializedObject.FindProperty("laserLines");
        lasers = serializedObject.FindProperty("lasers");
        ringRotationSpeed = serializedObject.FindProperty("ringRotationSpeed");
        minRotateTime = serializedObject.FindProperty("minRotateTime");
        maxRotateTime = serializedObject.FindProperty("maxRotateTime");
        laserLength = serializedObject.FindProperty("laserLength");
        damageRate = serializedObject.FindProperty("damageRate");
        randomLaserTime = serializedObject.FindProperty("randomLaserTime");
        shootFireBallRate = serializedObject.FindProperty("shootFireBallRate");
        laserDmg = serializedObject.FindProperty("laserDmg");
        hitMask = serializedObject.FindProperty("hitMask");

        useEffectWhenDie = serializedObject.FindProperty("useEffectWhenDie");
        deathEffect = serializedObject.FindProperty("deathEffect");
        explosionDmg = serializedObject.FindProperty("explosionDmg");
        explosionForce = serializedObject.FindProperty("explosionForce");
        explosionTime = serializedObject.FindProperty("explosionTime");
        splashRadius = serializedObject.FindProperty("splashRadius");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(bossType);
        EditorGUILayout.PropertyField(enemyTakeDmg);

        switch ((BossAttack.Boss)bossType.enumValueIndex)
        {
            case BossAttack.Boss.MechaGolem:
                BossAttack bossAttack = (BossAttack)target;

                DisplaySettingForMechaGolem();

                // Display a custom button in the Inspector
                if (GUILayout.Button("Calculate Rotation"))
                {
                    // Code to be executed when the button is clicked
                    bossAttack.CalculateRotation();
                }
                break;
            case BossAttack.Boss.Crab:
                DisplaySettingForCrab();
                break;
        }

        EditorGUILayout.PropertyField(useEffectWhenDie);
        if (useEffectWhenDie.boolValue)
        {
            DisplayDeathEffectSetting();
        }

        serializedObject.ApplyModifiedProperties();

    }

    void DisplaySettingForMechaGolem()
    {
        EditorGUILayout.PropertyField(rotation);
        EditorGUILayout.PropertyField(numberOfShootPoint);
        EditorGUILayout.PropertyField(radiusMultiplier);
        EditorGUILayout.PropertyField(shootPointList, true);

        EditorGUILayout.PropertyField(animator);
        EditorGUILayout.PropertyField(attackClip);
        EditorGUILayout.PropertyField(ring);
        EditorGUILayout.PropertyField(drone, true);
        EditorGUILayout.PropertyField(laserLines, true);
        EditorGUILayout.PropertyField(lasers, true);
        EditorGUILayout.PropertyField(ringRotationSpeed);
        EditorGUILayout.PropertyField(minRotateTime);
        EditorGUILayout.PropertyField(maxRotateTime);
        EditorGUILayout.PropertyField(laserLength);
        EditorGUILayout.PropertyField(damageRate);
        EditorGUILayout.PropertyField(randomLaserTime);
        EditorGUILayout.PropertyField(shootFireBallRate);
        EditorGUILayout.PropertyField(laserDmg);
        EditorGUILayout.PropertyField(hitMask);
    }

    void DisplaySettingForCrab()
    {
        EditorGUILayout.PropertyField(firePoint);
        EditorGUILayout.PropertyField(gunHolderPos);
        EditorGUILayout.PropertyField(bullet);
        EditorGUILayout.PropertyField(gatling);
        EditorGUILayout.PropertyField(flamethrower);
        EditorGUILayout.PropertyField(missleLauncher);
        EditorGUILayout.PropertyField(flameParticle, true);
        EditorGUILayout.PropertyField(flameDetectPlayer);
        EditorGUILayout.PropertyField(bulletDamage);
        EditorGUILayout.PropertyField(flameDamage);
        EditorGUILayout.PropertyField(gunHolderRotateSpeed);
        EditorGUILayout.PropertyField(fireRate);
        EditorGUILayout.PropertyField(flameFireRate);
        EditorGUILayout.PropertyField(spreadAngle);
        EditorGUILayout.PropertyField(numberOfBullet);
        EditorGUILayout.PropertyField(maxBulletSpeed);
        EditorGUILayout.PropertyField(minBulletSpeed);

        EditorGUILayout.PropertyField(minXpos);
        EditorGUILayout.PropertyField(maxXpos);
        EditorGUILayout.PropertyField(minYpos);
        EditorGUILayout.PropertyField(maxYpos);
        EditorGUILayout.PropertyField(missleSpeed);
        EditorGUILayout.PropertyField(timeBtwLaunchingMissile);
        EditorGUILayout.PropertyField(maxNumberMissle);
        EditorGUILayout.PropertyField(minNumberMissle);
        EditorGUILayout.PropertyField(missleIndicator);
        EditorGUILayout.PropertyField(missle);
        EditorGUILayout.PropertyField(missleSpawnPoint);
    }

    void DisplayDeathEffectSetting()
    {
        EditorGUILayout.PropertyField(deathEffect);
        EditorGUILayout.PropertyField(explosionDmg);
        EditorGUILayout.PropertyField(explosionForce);
        EditorGUILayout.PropertyField(explosionTime);
        EditorGUILayout.PropertyField(splashRadius);
    }
}

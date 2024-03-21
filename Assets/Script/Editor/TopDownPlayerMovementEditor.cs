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
    SerializedProperty weaponMask;
    SerializedProperty buffMask;
    SerializedProperty itemMask;
    SerializedProperty pickUpRadius;
    SerializedProperty boxCollider;
    SerializedProperty gunPos;
    SerializedProperty buffPos;
    SerializedProperty itemPos;
    SerializedProperty animator;
    SerializedProperty rb;
    SerializedProperty speed;
    SerializedProperty playerDashAfterImage;
    SerializedProperty numberOfAfterImage;
    SerializedProperty dashLength;
    SerializedProperty dashCooldown;
    SerializedProperty dashSpeed;
    SerializedProperty dashBg;
    SerializedProperty dashMask;
    SerializedProperty maxDaskMaskValue;
    SerializedProperty shieldBg;
    SerializedProperty shieldMask;
    SerializedProperty maxShieldMaskValue;
    SerializedProperty floatingText;
    SerializedProperty floatingTextPos;
    SerializedProperty randomFloatingTextPos;
    SerializedProperty flashDuration;
    SerializedProperty playerSprite;
    SerializedProperty hurtMat;
    SerializedProperty magnitude;
    SerializedProperty shield;
    SerializedProperty shieldOnTime;
    SerializedProperty shieldCoolDownTime;

    SerializedProperty healthSlider;
    SerializedProperty armourSlider;
    SerializedProperty energySlider;
    SerializedProperty maxHealth;
    SerializedProperty maxArmour;
    SerializedProperty maxEnergy;
    SerializedProperty regenerateArmourRate;
    SerializedProperty regenerateEnegeyRate;
    SerializedProperty regenerateHealthRate;
    SerializedProperty weaponStatCanvas;
    SerializedProperty buffDescCanvas;
    SerializedProperty startNewWaveTxt;
    SerializedProperty weaponDmg;
    SerializedProperty weaponRoF;
    SerializedProperty weaponEnergyConsume;
    SerializedProperty weaponCriticalHit;
    SerializedProperty weaponAccuracy;
    SerializedProperty buffDesc;
    SerializedProperty healthText;
    SerializedProperty armourText;
    SerializedProperty energyText;
    SerializedProperty coinText;

    SerializedProperty buffStatPanel;
    SerializedProperty maxHealthText;
    SerializedProperty maxArmourText;
    SerializedProperty maxEnergyText;
    SerializedProperty weaponDamageText;
    SerializedProperty weaponRofText;
    SerializedProperty criticalChanceText;
    SerializedProperty criticalDamageText;
    SerializedProperty weaponAccuracyText;
    SerializedProperty movementSpeedText;
    SerializedProperty numberOfBulletText;

    bool showGeneralSetting, showUiSetting = false, showBuffStatUiSetting = false;

    private void OnEnable()
    {
        cam = serializedObject.FindProperty("cam");
        camFollowPos = serializedObject.FindProperty("camFollowPos");
        mainCam = serializedObject.FindProperty("mainCam");
        offset = serializedObject.FindProperty("offset");
        pickUp = serializedObject.FindProperty("pickUp");
        weaponMask = serializedObject.FindProperty("weaponMask");
        buffMask = serializedObject.FindProperty("buffMask");
        itemMask = serializedObject.FindProperty("itemMask");
        pickUpRadius = serializedObject.FindProperty("pickUpRadius");
        boxCollider = serializedObject.FindProperty("boxCollider");
        gunPos = serializedObject.FindProperty("gunPos");
        buffPos = serializedObject.FindProperty("buffPos");
        itemPos = serializedObject.FindProperty("itemPos");
        animator = serializedObject.FindProperty("animator");
        rb = serializedObject.FindProperty("rb");
        speed = serializedObject.FindProperty("speed");
        dashBg = serializedObject.FindProperty("dashBg");
        dashMask = serializedObject.FindProperty("dashMask");
        maxDaskMaskValue = serializedObject.FindProperty("maxDaskMaskValue");
        shieldBg = serializedObject.FindProperty("shieldBg");
        shieldMask = serializedObject.FindProperty("shieldMask");
        maxShieldMaskValue = serializedObject.FindProperty("maxShieldMaskValue");
        playerDashAfterImage = serializedObject.FindProperty("playerDashAfterImage");
        numberOfAfterImage = serializedObject.FindProperty("numberOfAfterImage");
        dashLength = serializedObject.FindProperty("dashLength");
        dashCooldown = serializedObject.FindProperty("dashCooldown");
        dashSpeed = serializedObject.FindProperty("dashSpeed");
        floatingText = serializedObject.FindProperty("floatingText");
        floatingTextPos = serializedObject.FindProperty("floatingTextPos");
        randomFloatingTextPos = serializedObject.FindProperty("randomFloatingTextPos");
        flashDuration = serializedObject.FindProperty("flashDuration");
        playerSprite = serializedObject.FindProperty("playerSprite");
        hurtMat = serializedObject.FindProperty("hurtMat");
        magnitude = serializedObject.FindProperty("magnitude");
        shield = serializedObject.FindProperty("shield");
        shieldOnTime = serializedObject.FindProperty("shieldOnTime");
        shieldCoolDownTime = serializedObject.FindProperty("shieldCoolDownTime");

        healthSlider = serializedObject.FindProperty("healthSlider");
        armourSlider = serializedObject.FindProperty("armourSlider");
        energySlider = serializedObject.FindProperty("energySlider");
        maxHealth = serializedObject.FindProperty("maxHealth");
        maxArmour = serializedObject.FindProperty("maxArmour");
        maxEnergy = serializedObject.FindProperty("maxEnergy");
        regenerateArmourRate = serializedObject.FindProperty("regenerateArmourRate");
        regenerateEnegeyRate = serializedObject.FindProperty("regenerateEnegeyRate");
        regenerateHealthRate = serializedObject.FindProperty("regenerateHealthRate");
        weaponStatCanvas = serializedObject.FindProperty("weaponStatCanvas");
        buffDescCanvas = serializedObject.FindProperty("buffDescCanvas");
        startNewWaveTxt = serializedObject.FindProperty("startNewWaveTxt");
        weaponDmg = serializedObject.FindProperty("weaponDmg");
        weaponRoF = serializedObject.FindProperty("weaponRoF");
        weaponEnergyConsume = serializedObject.FindProperty("weaponEnergyConsume");
        weaponCriticalHit = serializedObject.FindProperty("weaponCriticalHit");
        weaponAccuracy = serializedObject.FindProperty("weaponAccuracy");
        buffDesc = serializedObject.FindProperty("buffDesc");
        healthText = serializedObject.FindProperty("healthText");
        armourText = serializedObject.FindProperty("armourText");
        energyText = serializedObject.FindProperty("energyText");
        coinText = serializedObject.FindProperty("coinText");

        buffStatPanel = serializedObject.FindProperty("buffStatPanel");
        maxHealthText = serializedObject.FindProperty("maxHealthText");
        maxArmourText = serializedObject.FindProperty("maxArmourText");
        maxEnergyText = serializedObject.FindProperty("maxEnergyText");
        weaponDamageText = serializedObject.FindProperty("weaponDamageText");
        weaponRofText = serializedObject.FindProperty("weaponRofText");
        criticalChanceText = serializedObject.FindProperty("criticalChanceText");
        criticalDamageText = serializedObject.FindProperty("criticalDamageText");
        weaponAccuracyText = serializedObject.FindProperty("weaponAccuracyText");
        movementSpeedText = serializedObject.FindProperty("movementSpeedText");
        numberOfBulletText = serializedObject.FindProperty("numberOfBulletText");


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

        showBuffStatUiSetting = EditorGUILayout.BeginFoldoutHeaderGroup(showBuffStatUiSetting, "Buff Stat UI Setting");
        if (showBuffStatUiSetting)
        {
            ShowBuffStatUiSetting();
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
        EditorGUILayout.PropertyField(weaponMask);
        EditorGUILayout.PropertyField(buffMask);
        EditorGUILayout.PropertyField(itemMask);
        EditorGUILayout.PropertyField(pickUpRadius);
        EditorGUILayout.PropertyField(boxCollider);
        EditorGUILayout.PropertyField(gunPos);
        EditorGUILayout.PropertyField(buffPos);
        EditorGUILayout.PropertyField(itemPos);
        EditorGUILayout.PropertyField(animator);
        EditorGUILayout.PropertyField(rb);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(playerDashAfterImage);
        EditorGUILayout.PropertyField(numberOfAfterImage);
        EditorGUILayout.PropertyField(dashLength);
        EditorGUILayout.PropertyField(dashCooldown);
        EditorGUILayout.PropertyField(dashSpeed);
        EditorGUILayout.PropertyField(dashBg);
        EditorGUILayout.PropertyField(dashMask);
        EditorGUILayout.PropertyField(maxDaskMaskValue);
        EditorGUILayout.PropertyField(shieldBg);
        EditorGUILayout.PropertyField(shieldMask);
        EditorGUILayout.PropertyField(maxShieldMaskValue);
        EditorGUILayout.PropertyField(floatingText);
        EditorGUILayout.PropertyField(floatingTextPos);
        EditorGUILayout.PropertyField(randomFloatingTextPos);
        EditorGUILayout.PropertyField(flashDuration);
        EditorGUILayout.PropertyField(playerSprite);
        EditorGUILayout.PropertyField(hurtMat);
        EditorGUILayout.PropertyField(magnitude);
        EditorGUILayout.PropertyField(shield);
        EditorGUILayout.PropertyField(shieldOnTime);
        EditorGUILayout.PropertyField(shieldCoolDownTime);
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
        EditorGUILayout.PropertyField(regenerateHealthRate);
        EditorGUILayout.PropertyField(weaponStatCanvas);
        EditorGUILayout.PropertyField(startNewWaveTxt);
        EditorGUILayout.PropertyField(weaponDmg);
        EditorGUILayout.PropertyField(weaponRoF);
        EditorGUILayout.PropertyField(weaponEnergyConsume);
        EditorGUILayout.PropertyField(weaponCriticalHit);
        EditorGUILayout.PropertyField(weaponAccuracy);
        EditorGUILayout.PropertyField(healthText);
        EditorGUILayout.PropertyField(armourText);
        EditorGUILayout.PropertyField(energyText);
        EditorGUILayout.PropertyField(coinText);


    }

    void ShowBuffStatUiSetting()
    {
        EditorGUILayout.PropertyField(buffDescCanvas);
        EditorGUILayout.PropertyField(buffDesc);
        EditorGUILayout.PropertyField(buffStatPanel);
        EditorGUILayout.PropertyField(maxHealthText);
        EditorGUILayout.PropertyField(maxArmourText);
        EditorGUILayout.PropertyField(maxEnergyText);
        EditorGUILayout.PropertyField(weaponDamageText);
        EditorGUILayout.PropertyField(weaponRofText);
        EditorGUILayout.PropertyField(criticalChanceText);
        EditorGUILayout.PropertyField(criticalDamageText);
        EditorGUILayout.PropertyField(weaponAccuracyText);
        EditorGUILayout.PropertyField(movementSpeedText);
        EditorGUILayout.PropertyField(numberOfBulletText);
    }
}

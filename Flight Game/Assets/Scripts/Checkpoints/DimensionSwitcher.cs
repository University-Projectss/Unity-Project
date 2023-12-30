using System;
using System.Linq;
using UnityEngine;

public class DimensionSwitcher : MonoBehaviour
{
    public enum DimensionOption
    {
        IceMountains,
        Toxic,
        StarryNight
    }

    public DimensionOption currentDimension = DimensionOption.IceMountains;

    Material terrainMaterial;

    private void Start()
    {
        terrainMaterial.color = Color.white;
    }

    private void Awake()
    {
        terrainMaterial = Resources.Load("Materials/Snow", typeof(Material)) as Material;
    }

    public void SwitchDimension()
    {
        // Initialize dimension properties with default values
        Color fogColor = Color.black, terrainColor = Color.black;
        Material skyboxMaterial = null;
        float fogDensity = 0, ambientIntensity = 0;

        currentDimension = ChooseRandomDimension();

        switch (currentDimension)
        {
            case DimensionOption.IceMountains:
                ColorUtility.TryParseHtmlString("#5AB1FF", out fogColor);
                terrainColor = Color.white;
                skyboxMaterial = Resources.Load("Materials/Skybox Cubemap Extended Day", typeof(Material)) as Material;
                fogDensity = 0.018f;
                ambientIntensity = 1;
                break;

            case DimensionOption.Toxic:
                ColorUtility.TryParseHtmlString("#FAFAC5", out fogColor);
                ColorUtility.TryParseHtmlString("#670516", out terrainColor);
                skyboxMaterial = Resources.Load("Materials/Skybox Toxic", typeof(Material)) as Material;
                fogDensity = 0.016f;
                ambientIntensity = 1.85f;
                break;

            case DimensionOption.StarryNight:
                ColorUtility.TryParseHtmlString("#58486A", out fogColor);
                terrainColor = Color.white;
                skyboxMaterial = Resources.Load("Materials/Skybox Cubemap Extended Night", typeof(Material)) as Material;
                fogDensity = 0.02f;
                ambientIntensity = 1;
                break;
        }

        ChangeSceneDetails(
            fogColor: fogColor,
            terrainColor: terrainColor,
            skyboxMaterial: skyboxMaterial,
            fogDensity: fogDensity,
            ambientIntensity: ambientIntensity
        );
    }

    private DimensionOption ChooseRandomDimension()
    {
        // Get all values from the DimensionOption enum and remove the current dimension
        var allDimensions = ((DimensionOption[])Enum
            .GetValues(typeof(DimensionOption)))
            .Where(dimension => !dimension.Equals(currentDimension))
            .ToList();

        // Choose a random dimension option
        return allDimensions[UnityEngine.Random.Range(0, allDimensions.Count)];
    }

    private void ChangeSceneDetails(
        Color fogColor,
        Color terrainColor,
        Material skyboxMaterial,
        float fogDensity,
        float ambientIntensity)
    {
        terrainMaterial.color = terrainColor;
        RenderSettings.fogColor = fogColor;
        RenderSettings.skybox = skyboxMaterial;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.ambientIntensity = ambientIntensity;
    }
}

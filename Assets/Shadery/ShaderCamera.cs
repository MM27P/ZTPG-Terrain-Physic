using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderCamera : MonoBehaviour
{
    public Material shaderMaterial;
    // Start is called before the first frame update

    //PrzemoPart
    public List<Vector2> crakPositions;
    public int crack_density = 100;

    public float range;
    public float offset;
    private int crackNumber;
    private float[] Random_lenth;
    private float[] Random_angle;
    private float[] Line_Factor_Y;
    private float[] Line_Factor_X;
    private float[] Far_Point_Y;
    private float[] Far_Point_X;
    //

    void Start()
    {
        Random_lenth = new float[crack_density * crakPositions.Count];
        Random_angle = new float[crack_density * crakPositions.Count];

        Line_Factor_Y = new float[crack_density * crakPositions.Count];
        Line_Factor_X = new float[crack_density * crakPositions.Count];

        Far_Point_Y = new float[crack_density * crakPositions.Count];
        Far_Point_X = new float[crack_density * crakPositions.Count];

        for (int i = 0; i < crack_density * crakPositions.Count; ++i)
        {
            Random_lenth[i] = UnityEngine.Random.value;
            Random_angle[i] = (float)Math.PI * 2*i/ crack_density;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        crackNumber = shaderMaterial.GetInt("_crack_number");
        float[] centerX = new float[crackNumber];
        float[] centerY = new float[crackNumber];
        float[] Lenght = new float[crack_density * crakPositions.Count];
        float force = shaderMaterial.GetInt("_Force");
        float b1 = 1, b2 = 1, wx, wy, w;

        for (int j = 0; j < crackNumber; j++)
        {

            centerX[j] = crakPositions[j].x;
            centerY[j] = crakPositions[j].y;

            for (int i = 0; i < crack_density; ++i)
            {
                Lenght[j * crack_density + i] = Random_lenth[j * crack_density + i] * range * force + offset;
                Far_Point_X[j * crack_density + i] = centerX[j] + Lenght[j * crack_density + i] * (float)Math.Cos(Random_angle[j * crack_density + i]);
                Far_Point_Y[j * crack_density + i] = centerY[j] + Lenght[j * crack_density + i] * (float)Math.Sin(Random_angle[j * crack_density + i]);

                w = Far_Point_X[j * crack_density + i] * b2 - b1 * centerX[j]; //wyznacznik główny
                wx = Far_Point_Y[j * crack_density + i] * b2 - b1 * centerY[j];
                wy = Far_Point_X[j * crack_density + i] * centerY[j] - Far_Point_Y[j * crack_density + i] * centerX[j];

                Line_Factor_X[j * crack_density + i] = wx / w;
                Line_Factor_Y[j * crack_density + i] = wy / w;
            }
        }

        shaderMaterial.SetFloatArray("_Rlenth", Lenght);
        shaderMaterial.SetFloatArray("_Rangle", Random_angle);
        shaderMaterial.SetFloatArray("_Factor_Y", Line_Factor_Y);
        shaderMaterial.SetFloatArray("_Factor_X", Line_Factor_X);
        shaderMaterial.SetFloatArray("_Far_Y", Far_Point_Y);
        shaderMaterial.SetFloatArray("_Far_X", Far_Point_X);

        shaderMaterial.SetFloatArray("_x_center", centerX);
        shaderMaterial.SetFloatArray("_y_center", centerY);

        shaderMaterial.SetInt("_crack_density", crack_density);

        Graphics.Blit(source, destination, shaderMaterial);
    }
}

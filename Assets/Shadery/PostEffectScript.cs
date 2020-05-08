using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffectScript : MonoBehaviour
{
    public Material shaderMaterial;

    public float x_center = 1;
    public float y_center = 1;
    public int crack_density = 1;

    public float range;
    public float offset;
    private float[] Random_lenth;
    private float[] Random_angle;
    private float[] Line_Factor_Y;
    private float[] Line_Factor_X;
    private float[] Far_Point_Y;
    private float[] Far_Point_X;


    // Start is called before the first frame update
    void Start()
    {
        Random_lenth    = new float[crack_density];
        Random_angle    = new float[crack_density];

        Line_Factor_Y   = new float[crack_density];
        Line_Factor_X   = new float[crack_density];

        Far_Point_Y     = new float[crack_density];
        Far_Point_X     = new float[crack_density];

        for (int i = 0; i < crack_density; ++i)
        {
            Random_lenth[i] = UnityEngine.Random.value *range + offset ;
            Random_angle[i] = UnityEngine.Random.value * (float)Math.PI*2;

            Far_Point_X[i] = x_center + Random_lenth[i] * (float)Math.Cos(Random_angle[i]);
            Far_Point_Y[i] = y_center + Random_lenth[i] * (float)Math.Sin(Random_angle[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float b1=1, b2=1, wx, wy, w;

        for (int i = 0; i < crack_density; ++i)
        {
            w = Far_Point_X[i] * b2 - b1 * x_center; //wyznacznik główny
            wx = Far_Point_Y[i] * b2 - b1 * y_center;
            wy = Far_Point_X[i] * y_center - Far_Point_Y[i] * x_center;

            Line_Factor_X[i] = wx / w;
            Line_Factor_Y[i] = wy / w;
        }


        shaderMaterial.SetFloatArray("_Rlenth", Random_lenth);
        shaderMaterial.SetFloatArray("_Rangle", Random_angle);
        shaderMaterial.SetFloatArray("_Factor_Y", Line_Factor_Y);
        shaderMaterial.SetFloatArray("_Factor_X", Line_Factor_X);
        shaderMaterial.SetFloatArray("_Far_Y", Far_Point_Y);
        shaderMaterial.SetFloatArray("_Far_X", Far_Point_X);

        shaderMaterial.SetFloat("_x_center", x_center);
        shaderMaterial.SetFloat("_y_center", y_center);

        shaderMaterial.SetInt("_crack_density", crack_density);

        Graphics.Blit(source, destination, shaderMaterial);
    }
}

using UnityEngine;
using System.Collections;
using System;//! Sample terrain animator/generator
using System.IO;
using System.Collections.Generic;

public class SimpleTerrainGenerator : MonoBehaviour
{

    //Basic Vars
    private Terrain _myTerr;
    private TerrainData _myTerrData;
    private int _xRes;
    private int _yRes;
    float[,] _terrHeights;
    float[,] originalTerrainSectionHeight;


    //Dynamic anim terrain in runtime
    public int numberOfPasses = 5;
    public int radiusOfAnimation = 50; // Use this for initializationvoid
    public int maxStep = 5;
    public int minStep = 0;
    public int tileX;
    public int tileY;
    public int directX;
    public int directY;
    private int counter = 0;

    //Vars for generate map from file
    private Color[] map;
    public Texture2D noiseMap;

    //Vars for create title in terrain from image
    public string TextureTitleFilePath = "Assets\\TerrainImage.jpg";
    private Texture2D textTitle = null;

    //Flags
    public bool WriteTitleFlag = false;
    public bool isNoiseMap = false;
    public bool DynamicTerrainModificator = false;

    void Start()
    {

        // Get terrain and terrain data   handles
        _myTerr = GetComponent<Terrain>();
        _myTerrData = _myTerr.terrainData;


        // Get terrain dimensions in tiles (X tiles x Y tiles) 
        _xRes = _myTerrData.heightmapResolution;
        _yRes = _myTerrData.heightmapResolution;
        tileX = _xRes / 2;
        tileY = _yRes / 2;

        if (isNoiseMap && noiseMap != null)
        {
            isNoiseMap = true;
            map = noiseMap.GetPixels(0, 0, _xRes, _yRes);
        }
        else if(noiseMap == null)
        {
            isNoiseMap = false;
        }

        int randomTemp = UnityEngine.Random.Range(1, 4);

        switch (randomTemp)
        {
            case 1:
                directX = 1;
                directY = 1;
                break;
            case 2:
                directX = -1;
                directY = 1;
                break;
            case 3:
                directX = 1;
                directY = -1;
                break;
            case 4:
                directX = -1;
                directY = -1;
                break;
        }
        // Set heightmap
        RandomizeTerrain();

        if (TextureTitleFilePath!= null && WriteTitleFlag)
        {
            LoadImage();
            WriteImage();
        } 
    }

    // Update is called once per framevoid 
    void Update()
    {
        // Call animation function
        if (counter == 5)
        {
            counter = 0;
            CalculateStep();
        }

        counter++;

        if (DynamicTerrainModificator)
            AnimTerrain();
    }

    private void CalculateStep()
    {
        int stepX = UnityEngine.Random.Range(minStep, maxStep);
        int stepY = UnityEngine.Random.Range(minStep, maxStep);
        tileX = tileX + (directX * stepX);
        tileY = tileY + (directY * stepY);

        if (tileX > _xRes - radiusOfAnimation || tileX < 0)
        {
            directX = -directX;
            tileX = tileX + (directX * stepX);
        }

        if (tileY > _yRes - radiusOfAnimation || tileY < 0)
        {
            directY = -directY;
            tileY = tileX + (directY * stepY);
        }
    }

    // Set the terrain using noise pattern
    private void RandomizeTerrain()
    {

        // Extract entire heightmap (expensive!)
        _terrHeights = _myTerrData.GetHeights(0, 0, _xRes, _yRes);
        // STUDENT'S CODE //// ...
        float[] scale = new float[numberOfPasses];

        for (int k = 0; k < numberOfPasses; k++)
        {
            scale[k] = UnityEngine.Random.Range(4.0f, 16.0f);
        }
        for (int i = 0; i < _xRes; i++)
        {
            for (int j = 0; j < _yRes; j++)
            {
                if (!isNoiseMap)
                {
                    float xCoeff = (float)i / _xRes;
                    float yCoeff = (float)j / _yRes;

                    _terrHeights[i, j] = 0;

                    for (int k = 0; k < numberOfPasses; k++)
                    {
                        _terrHeights[i, j] += Mathf.PerlinNoise(xCoeff * scale[k], yCoeff * scale[k]);
                    }

                    _terrHeights[i, j] /= (float)numberOfPasses;
                }
                else
                {
                    _terrHeights[i, j] = map[i + j * _xRes].grayscale;
                    _terrHeights[i, j] /= (float)numberOfPasses;
                }
            }
        }
        // // Set terrain heights (_terrHeights[coordX, coordY] = heightValue) in a loop//
        // You can sample perlin's noise (Mathf.PerlinNoise (xCoeff, yCoeff)) usingcoefficients
        // between 0.0f and 1.0f// 
        // You can combine 2-3 layers of noise with different resolutions and amplitudes fora better effect// 
        // END OF STUDENT'S CODE //// Set entire heightmap (expensive!)

        _myTerrData.SetHeights(0, 0, _terrHeights);
        originalTerrainSectionHeight = _myTerrData.GetHeights(0, 0, _xRes, _yRes);
    }

    // Animate part of the terrainprivate 
    void AnimTerrain()
    {
        // STUDENT'S CODE ////
        // Extract PART of the terrain e.g. 40x40 tiles (select corner (x, y) and extractedpatch size)
        // GetHeights(5, 5, 10, 10) will extract 10x10 tiles at position (5, 5)
        //// Animate it using Time.time and trigonometric functions
        /////// 3d generalizaton of sinc(x) function can be used to create the teardrop effect(sinc(x) = sin(x) / x)
        /////// It is reasonable to store animated part of the terrain in temporary variable e.g.in RandomizeTerrain()
        ///// function. Later, in AnimTerrain() this temporary area can be combined withcalculated Z (height) value.
        ///// Make sure you make a deep copy instead of shallow one (Clone(), assign operator).
        ///// // Set PART of the terrain (use extraction parameters)
        /////// END OF STUDENT'S CODE //
        ///


        _terrHeights = _myTerrData.GetHeights(tileX, tileY, radiusOfAnimation, radiusOfAnimation);
        Vector2 middle = new Vector2(radiusOfAnimation, radiusOfAnimation);

        for (int i = 0; i < radiusOfAnimation; i++)
        {
            for (int j = 0; j < radiusOfAnimation; j++)
            {
                Vector2 point = new Vector2(i, j);
                double distance = Vector2.Distance(point, middle);
                double difference = (radiusOfAnimation - distance) / radiusOfAnimation;

                if (difference < 0)
                    difference = 0;
                _terrHeights[i, j] = (float)(originalTerrainSectionHeight[tileX + i, tileY + j] * (Math.Sin(Time.time + distance / 10) / 2f) * difference) + originalTerrainSectionHeight[tileX + i, tileY + j];
            }
        }
        _myTerrData.SetHeights(tileX, tileY, _terrHeights);
    }

    private void LoadImage()
    {
        if (File.Exists(TextureTitleFilePath))
        {
            textTitle = new Texture2D(2, 2);
            textTitle.LoadImage(File.ReadAllBytes(TextureTitleFilePath)); //..this will auto-resize the texture dimensions.
        }
    }

    void WriteImage()
    {

        // Get terrain dimensions in tiles (X tiles x Y tiles) 
        var imageX = textTitle.height;
        var imageY = textTitle.width;
        var map = textTitle.GetPixels(0, 0, imageX, imageY);

        var terrainHeights = _myTerrData.GetHeights(0, 0, imageX, imageY);

        for (int i = 0; i < imageX; i++)
        {
            for (int j = 0; j < imageY; j++)
            {
                terrainHeights[j, i] = map[i + j * imageX].grayscale;
            }
        }
        _myTerrData.SetHeights(0, 0, terrainHeights);
    }

}
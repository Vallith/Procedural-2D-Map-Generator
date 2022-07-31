using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{

    static float GetFalloffValue(int currentWidth, int currentHeight, int width, int height, float falloffGradient, float falloffSize)
    {
        if (currentWidth == 5 && currentHeight == 5)
        {

        }
        float value = 0f;

        float x = currentHeight / (float)height * 2 - 1;
        float y = currentWidth / (float)width * 2 - 1;

        value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
        value = Evaluate(value, falloffGradient, falloffSize);

        return value;
    }

    public static float[,] GenerateSquareFalloffMap(int width, int height, float falloffGradient, float falloffSize)
    {
        float[,] map = new float[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[j, i] = GetFalloffValue(j, i, width, height, falloffGradient, falloffSize);
            }
        }
        return map;
    }

    public static float[,] GenerateRadialFalloffMap(int width, int height, float falloffGradient, float falloffSize, float radius)
    {
        float [,] map = new float[width, height];

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                float value = GetFalloffValue(j, i, width, height, falloffGradient, falloffSize);
                map[j, i] = RadialFallOff(value, radius, j, i, width / 2f, height / 2f);
            }
        }
        return map;
    }

    public static float[,] GenerateFeatheredRadialFalloffMap(int width, int height, float falloffGradient, float falloffSize, float radius1, float radius2)
    {
        float[,] map = new float[width, height];

        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                float value = GetFalloffValue(j, i, width, height, falloffGradient, falloffSize);
                map[j, i] = FeatheredRadialFallOff(value, radius1, radius2, j, i, width / 2f, height / 2f);
            }
        }
        return map;
    }

    /// value - The calculated value to process
    /// radius - The distance from center to calculate falloff distance
    /// x - The x-coordinate of the value position
    /// y - The y-coordinate of the value position
    /// cx - The x-coordinate of the center position
    /// cy - The y-coordinate of the center position
    public static float RadialFallOff(float value, float radius, int x, int y, float cx, float cy)
    {
        float dx = cx - x;
        float dy = cy - y;
        float distSqr = dx * dx + dy * dy;
        float radSqr = radius * radius;

        if (distSqr > radSqr) return 1f;
        return 1f - value;
    }

    /// value - The calculated value to process
    /// innerRadius - The distance from center to start feathering
    /// outerRadius - The distance from center to fully fall off
    /// x - The x-coordinate of the value position
    /// y - The y-coordinate of the value position
    /// cx - The x-coordinate of the center position
    /// cy - The y-coordinate of the center position
    public static float FeatheredRadialFallOff(float value, float innerRadius, float outerRadius, int x, int y, float cx, float cy)
    {
        float dx = cx - x;
        float dy = cy - y;
        float distSqr = dx * dx + dy * dy;
        float iRadSqr = innerRadius * innerRadius;
        float oRadSqr = outerRadius * outerRadius;

        if (distSqr >= oRadSqr) return 1f;
        if (distSqr <= iRadSqr) return 1f - value;

        float dist = Mathf.Sqrt(distSqr);
        float t = Mathf.InverseLerp(innerRadius, outerRadius, dist);
        // Use t with whatever easing you want here, or leave it as is for linear easing
        return value * t;
    }

    static float Evaluate(float value, float a, float b)
    {
        float v = Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
        if(v >= 0 && v <= 1) { return v; }
        else { return 1f; }
    }
}

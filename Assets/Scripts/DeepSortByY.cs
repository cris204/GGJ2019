﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepSortByY : MonoBehaviour
{
    private const int IsometricRangePerYUnit = 10;

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
    }
}

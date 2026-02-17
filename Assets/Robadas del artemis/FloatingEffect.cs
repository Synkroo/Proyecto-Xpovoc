using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float velocidad = 5;
    public float cantidad = 0.1f;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        float movimientoFinal = Mathf.Sin(Time.time * velocidad) * cantidad;
        transform.localPosition = posicionInicial + new Vector3(0, movimientoFinal, 0);
    }
}

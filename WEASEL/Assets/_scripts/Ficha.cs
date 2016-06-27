using UnityEngine;
using System.Collections;

public class Ficha : MonoBehaviour {
    private Vector3 originalPosition;//variable que guarda el transform inicial de la ficha
    private Vector3 previousPosition;//variable que guarda la posicion previa de la ficha
    private bool block = false;
    private bool isSpace = false; 

    //propiedad que nos da el valor o asigna de la posicion previa de la ficha
    public Vector3 PreviousPosition
    {
        get
        {
            return previousPosition;
        }

        set
        {
            previousPosition = value;
        }
    }

    //propiedad que da el valor o nos permite modificarlo de la variable booleana que se encarga de decir si una ficha esta bloqueada o no
    public bool Block
    {
        get
        {
            return block;
        }

        set
        {
            block = value;
        }
    }


    public bool IsSpace
    {
        get
        {
            return isSpace;
        }

        set
        {
            isSpace = value;
        }
    }




    // Use this for initialization
    void Awake () {
        originalPosition = this.transform.position;//constructor guarda la posicion inicial de la ficha
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;//funcion que devuelve la posicion original de la ficha
    }
}

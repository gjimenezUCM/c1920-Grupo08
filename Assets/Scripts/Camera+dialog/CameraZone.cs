﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Esta componente detecta cuando el CameraPosition entra en la zona de influencia del objeto asociado. El 
 objeto necesitará un collider habilitado como trigger (no importa el tipo de collider). 
 Al entrar el jugador en esta zona, la cámara se desplaza hasta un punto predefinido por la posición del hijo
 de este objeto, designado CameraAnchor. Además, cambiará el tamaño de la cámara a una predefinida para esta
 zona en particular.
 Al salir de la zona, los cambios se revierten.*/

public class CameraZone : MonoBehaviour
{
    //El tamaño que adquirirá la cámara cuando esté en esta zona
    [SerializeField]
    float CameraSize = 5f;
    //La velocidad a la que la cámara se moverá hasta y desde esta zona
    [SerializeField]
    float snapSpeed = 1f;
    [SerializeField]
    bool UseAnchor = true;
    //El desplazamiento vertical de la cámara (solo usado en zonas sin ancla)
    [SerializeField]
    float offset=0f;
    //La posición donde se colocará la cámara en esta zona
    Vector3 anchor;
    //Referencia al objeto que contiene a la cámara
    GameObject cam;
    float defaultSize;
    //Los scripts que tendremos que acceder de la cámara
    SmoothMovement movement;
    CameraSize camSize;
    
    void Start()
    {
        //Guardamos la posición de anclaje de la cámara
        anchor = transform.GetChild(0).GetComponent<Transform>().position;
        cam = SmoothMovement.mainCamera.gameObject;
        //Guardamos referencias a los scripts necesarios de la cámara
        movement = cam.gameObject.GetComponent<SmoothMovement>();
        camSize = cam.gameObject.transform.GetChild(0).GetComponent<CameraSize>();
        //Guardamos el tamaño base de la cámara
        defaultSize = camSize.GetSize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision detected");
        if (UseAnchor)
        {
            Debug.Log("Anchor used: "+ anchor);
            //Movemos la cámara hasta el ancla
            movement.MoveToAnchor(anchor, snapSpeed);
        }

        //Cambiamos el tamaño de la cámara a la necesaria para esta zona
        camSize.ChangeSize(CameraSize, snapSpeed);
        movement.EnterZone();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("inside " + movement.InsideZones() + " zones");
        movement.ExitZone();
        //No es necesario asignar cam: para poder salir del trigger tiene que haber primero entrado
        if (movement.InsideZones()==0)
        {
            Debug.Log("defaultsize = " + defaultSize);
            //Movemos la cámara a su posición original relativa al jugador
            movement.ReturnToPlayer();
            //Devolvemos la cámara a su tamaño original
            camSize.ChangeSize(defaultSize, snapSpeed);
        }
    }
}
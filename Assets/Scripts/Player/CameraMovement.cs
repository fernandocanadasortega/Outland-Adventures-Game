using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Made by Cañadas Ortega, Fernando
 * 2º Desarrollo de aplicaciones multiplataformas, San José
 */

/// <summary>
/// This class is in charge of set the camera movement boundaries
/// </summary>
public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private float topLeftX, topLeftY, bottomRightX, bottomRightY;
    private bool chasingCamera;
    private float smoothTime;

    private Vector2 currentVelocity;

    /// <summary>
    /// Function that is called right after the scene is loaded. Check if the Camera chase option is on or not, and if is on get the smooth time value.
    /// </summary>
    private void Awake()
    {
        int cameraChaseOption = PlayerPrefs.GetInt("CameraChase");

        if (cameraChaseOption == 0)
        {
            chasingCamera = false;
        }
        else
        {
            chasingCamera = true;

            smoothTime = PlayerPrefs.GetFloat("CameraChaseTime");
        }
    }

    /// <summary>
    /// Update is called once per frame. Move the camera according to the map boundaries, if the camera chase option is on then wait some time until the camera follow
    /// the character
    /// 
    /// </summary>
    void Update()
    {
        if (chasingCamera)
        {
            // Wait a time before the camera start chasing the character
            float smoothedCameraPositionX = Mathf.Round(Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref currentVelocity.x, smoothTime) * 100) / 100;
            float smoothedCameraPositionY = Mathf.Round(Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref currentVelocity.y, smoothTime) * 100) / 100;

            // Move the camera according to the boundaries of the map (topLeftXY and bottomRightXY)
            transform.position = new Vector3(
                Mathf.Clamp(smoothedCameraPositionX, topLeftX, bottomRightX),
                Mathf.Clamp(smoothedCameraPositionY, bottomRightY, topLeftY),
                transform.position.z
                );
        }
        // Move the camera according to the boundaries of the map (topLeftXY and bottomRightXY)
        else
        {
            transform.position = new Vector3(
            Mathf.Clamp(player.transform.position.x, topLeftX, bottomRightX),
            Mathf.Clamp(player.transform.position.y, bottomRightY, topLeftY),
            transform.position.z
            );
        }
    }

    /// <summary>
    /// Set the camera boundaries according to the map you will teleport
    /// </summary>
    /// <param name="destinationMap">GameObject, map to which you go</param>
    public void setCameraBounds(GameObject destinationMap)
    {
        // Transfor GameObject to TiledMap object
        Tiled2Unity.TiledMap destinationMapConfig = destinationMap.GetComponent<Tiled2Unity.TiledMap>();
        float cameraSize = Camera.main.orthographicSize;

        // Transform position of tiled maps is on Top Left
        topLeftX = destinationMapConfig.transform.position.x + cameraSize;
        topLeftY = destinationMapConfig.transform.position.y - cameraSize;
        bottomRightX = destinationMapConfig.transform.position.x + destinationMapConfig.NumTilesWide - cameraSize;
        bottomRightY = destinationMapConfig.transform.position.y - destinationMapConfig.NumTilesHigh + cameraSize;

        setFirstCameraFrame();
    }

    /// <summary>
    /// Make the camera teleport to you immediately when the map charges
    /// </summary>
    private void setFirstCameraFrame()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        try
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
        }
        catch (System.Exception)
        {
            Debug.LogError("Camera Transform Critical Error");
        }
    }

    /// <summary>
    /// Check if the Camera chase option is on or not, and if is on get the smooth time value, is executed when you change the camera setting in the pause menu
    /// </summary>
    public void setCameraOptions()
    {
        int cameraChaseOption = PlayerPrefs.GetInt("CameraChase");

        if (cameraChaseOption == 0)
        {
            chasingCamera = false;
        }
        else
        {
            chasingCamera = true;

            smoothTime = PlayerPrefs.GetFloat("CameraChaseTime");
        }
    }
}

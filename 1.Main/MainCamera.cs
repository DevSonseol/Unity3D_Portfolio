using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum CameraMode 
{
    None = -1,  
    Lobby,
    Ingame,
    Count
}

public partial class Main : MonoBehaviour
{
    [Header("Camera")]
    public Light directionalLight;
    public Camera mainCam;
    public CinemachineVirtualCamera cineVirtualCam;

    public GameObject cameraTarget;

    Renderer obstacleRenderer;
    Material obstacleMaterial;

    private CameraMode cameraMode = CameraMode.None;

    private RaycastHit[] hitBuffer = new RaycastHit[32];

    void OnUpdateCamera(float dt)
    {
        if (mainCam == null || cameraTarget == null) return;

        UpdateFollowingTarget(dt);
        ObstacleHide(dt);
    }

    void ChangeCameraMode(int type)
    {
        if (type == (int)CameraMode.Lobby)
        {

        }
        else if (type == (int)CameraMode.Ingame)
        {
            mainCam.orthographic = true;
            cineVirtualCam.transform.rotation = Quaternion.Euler(45f, 30f, 0);
        }
    }

    void UpdateFollowingTarget(float dt) {
        if (cameraTarget == null) return;
        
        cineVirtualCam.transform.position = cameraTarget.transform.position + Constants.IngameCameraoffeset;
    }
    
    void ObstacleHide(float dt)
    {
        float Distance = Vector3.Distance(mainCam.transform.position, cameraTarget.transform.position);

        Vector3 Direction = (cameraTarget.transform.position - mainCam.transform.position).normalized;

        RaycastHit hit;

        Main main = Main.instance;

        if (Physics.Raycast(mainCam.transform.position, Direction, out hit, Distance,LayerMask.GetMask("Wall")))
        {
            if (obstacleRenderer != null)
                obstacleRenderer = ChangeObstcleAlpha(obstacleRenderer, 1.0f, obstacleRenderer.material, false );
            
            obstacleRenderer = hit.collider.gameObject.GetComponentInChildren<Renderer>();

            if (obstacleRenderer != null)
                obstacleRenderer = ChangeObstcleAlpha(obstacleRenderer, 0.1f, obstacleRenderer.material, true);
            
        }
        else
        {
            if (obstacleRenderer != null)
                obstacleRenderer = ChangeObstcleAlpha(obstacleRenderer, 1.0f, obstacleRenderer.material, false );
        }
        
    }

    Renderer ChangeObstcleAlpha(Renderer obstacleRenderer, float alpha ,Material mat ,bool transparent)
    {
        Material Mat = obstacleRenderer.material;
        Color matColor = Mat.color;

        matColor.a = alpha;

        Mat.color = matColor;

        if(transparent)
        {
            Mat.SetOverrideTag("RenderType", "Transparent");
            Mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            Mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            Mat.SetInt("_ZWrite", 0);
            Mat.DisableKeyword("_ALPHATEST_ON");
            Mat.EnableKeyword("_ALPHABLEND_ON");
            Mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            Mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            Mat.SetFloat("_Mode", 3.0f);
        }
        else
        {
            Mat.SetOverrideTag("RenderType", "");
            Mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            Mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            Mat.SetInt("_ZWrite", 1);
            Mat.DisableKeyword("_ALPHATEST_ON");
            Mat.DisableKeyword("_ALPHABLEND_ON");
            Mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            Mat.renderQueue = -1;
            Mat.SetFloat("_Mode", 0.0f);
        }

        return obstacleRenderer;
    }
}

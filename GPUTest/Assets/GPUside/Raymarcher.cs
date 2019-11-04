
using UnityEngine;

public class Raymarcher : MonoBehaviour
{
    public ComputeShader raymarching;
    public int threadGroupX, threadGroupY, threadGroupZ;
    RenderTexture result;
    public Light dirLight;
    Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void CreateTexture()
    {
        if(result==null||result.width!=Screen.width||result.height!=Screen.height)
        {
            if(result!=null)
            {
                result.Release();
            }
            result = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            result.enableRandomWrite = true;
            result.Create();
            threadGroupX = Screen.width / 8;
            threadGroupY = Screen.height / 8;
            threadGroupZ = 1;
        }
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateTexture();
        int idkernel = raymarching.FindKernel("CSMain");
        raymarching.SetTexture(idkernel, "Result", result);
        raymarching.SetMatrix("cameraToWorld", _camera.cameraToWorldMatrix);
        raymarching.SetMatrix("cameraToProjectionInverse", _camera.projectionMatrix.inverse);
        raymarching.SetVector("directionalLight", 
            new Vector4(dirLight.transform.forward.x, dirLight.transform.forward.y, dirLight.transform.forward.z, dirLight.intensity));
        raymarching.Dispatch(idkernel, Mathf.CeilToInt(threadGroupX), Mathf.CeilToInt(threadGroupY), 1);
        Graphics.Blit(result, destination);
    }
    
}

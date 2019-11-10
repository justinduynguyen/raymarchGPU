
using UnityEngine;

public class Raymarcher : MonoBehaviour
{
    public ComputeShader raymarching;
    public int threadGroupX, threadGroupY, threadGroupZ;
    RenderTexture result;
    public Texture enviroment;
    Camera _camera;
    public Material antiAlias;
    //public bool pause;
    public Light dirLight;
    public Texture skybox;
     [Range(0,1)]
    public float roughness;
    RenderTexture _convered;
    int idkernel;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;
         idkernel = raymarching.FindKernel("CSMain");
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
            _convered = new RenderTexture(Screen.width, Screen.height, 0,
       RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _convered.enableRandomWrite = true;
            threadGroupX = Screen.width / 8;
            threadGroupY = Screen.height / 8;
            threadGroupZ = 1;
        }
    }
 
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateTexture();
      
        raymarching.SetTexture(idkernel, "Result", result);
        raymarching.SetMatrix("cameraToWorld", _camera.cameraToWorldMatrix);
        raymarching.SetMatrix("cameraToProjectionInverse", _camera.projectionMatrix.inverse);
        Texture depth = Shader.GetGlobalTexture("_CameraDepthTexture");
        raymarching.SetTexture(idkernel, "_depthTexture", depth);
        raymarching.SetVector("directionalLight",
            new Vector4(dirLight.transform.forward.x, dirLight.transform.forward.y, dirLight.transform.forward.z, dirLight.intensity));
        raymarching.SetTexture(idkernel, "_SkyboxTexture", skybox);
        raymarching.SetVector("lightColor",new Vector3(dirLight.color.r,dirLight.color.g,dirLight.color.b));
        raymarching.SetFloat("roughness", roughness);
        raymarching.SetFloat("_Time", Time.time);
        raymarching.SetTexture(idkernel,"_rasTexture",source);
      
        raymarching.Dispatch(idkernel, Mathf.CeilToInt(threadGroupX), Mathf.CeilToInt(threadGroupY), 1);
        Graphics.Blit(result, destination, antiAlias);     
          //  Graphics.Blit(_convered, destination);
           
        }
    }
    


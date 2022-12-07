using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ImageEffect : MonoBehaviour
{
    [SerializeField] protected Shader _shader;
    protected Material material;

    private void Awake() 
    {
        material = new Material(_shader);
    }

    protected virtual void OnRenderImage(RenderTexture src, RenderTexture dst) 
    {
        Graphics.Blit(src, dst, material);
    }
}

using UnityEngine;

public class ShieldColorController : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_Main_Color");

    public ParticleSystemRenderer[] PSRenderers;
    
    [ColorUsage(true, true)]
    public Color[] startColors;

    [Range(0f, 1f)]
    public float range = 1f;


    Vector3[] ColorHSV;

    static MaterialPropertyBlock block;

    public virtual void Start() {
        ColorHSV = new Vector3[PSRenderers.Length];
    }

    public virtual void Update() {

        if (block == null) {
            block = new MaterialPropertyBlock();
        }

        Color[] FinalColor = new Color[PSRenderers.Length];

        for (int i = 0; i < PSRenderers.Length; i++) {
            Color.RGBToHSV(startColors[i], out float H, out float S, out float V);
            ColorHSV[i] = new Vector3(H * range, S, V);
            FinalColor[i] = Color.HSVToRGB(ColorHSV[i].x, ColorHSV[i].y, ColorHSV[i].z, true);
            block.SetColor(baseColorId, FinalColor[i]);
            PSRenderers[i].SetPropertyBlock(block);
        }

    }

}

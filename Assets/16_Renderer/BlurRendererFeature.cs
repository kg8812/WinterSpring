using CatDarkGame.RendererFeature;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature {

    private static readonly string BLUR_SHADER_NAME = "Hidden/CustomRenderFeature/Gaussianblur";

    public string shaderTag_Drawpass = "Transparent";
    public ShaderTagId GetShaderTagID_Drawpass => new ShaderTagId(shaderTag_Drawpass);

    public RenderPassEvent passEvent = RenderPassEvent.AfterRenderingTransparents;
    [Range(0, 8)]
    public int blurStep = 8;

    private BlurDrawPass _blurDrawPass;
    private NormalDrawPass _normalDrawPass;

    private Material _material = null;

    // Create: Unity는 다음 이벤트에서 이 메서드를 호출합니다.
    // - 렌더러 기능(Renderer Feature)이 처음 로드될 때
    // - 렌더러 기능을 활성화하거나 비활성화하는 경우
    // - 렌더러 기능의 inspector 에서 property를 변경할 때
    public override void Create()
    {
        if (!_material) {
            Shader shader = Shader.Find(BLUR_SHADER_NAME);
            _material = CoreUtils.CreateEngineMaterial(shader);
        }
        _blurDrawPass = new BlurDrawPass(passEvent, _material);
        _normalDrawPass = new NormalDrawPass(passEvent + 1, GetShaderTagID_Drawpass);
    }

    //소멸자
    protected override void Dispose(bool disposing) {
        if (!disposing) return;
        if (_material) {
            CoreUtils.Destroy(_material);
            _material = null;
        }
        _blurDrawPass = null;
        _normalDrawPass = null;
    }

    // AddRenderPasses: Unity는 매 프레임마다, 각 카메라에 대해 한 번씩 이 메서드를 호출합니다.
    // 이 방법을 사용하면 ScriptableRenderPass 인스턴스를 스크립팅 가능한 렌더러에 삽입할 수 있습니다.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) 
    {
        UniversalAdditionalCameraData addCamera = renderingData.cameraData.camera.GetUniversalAdditionalCameraData();

        //랜더타입이 오버레이인 카메라가 하나밖에 없기 때문에 이 방법을 사용하지만, 문제가 생길 수 있음.
        if (addCamera.renderType == CameraRenderType.Overlay && blurStep != 0) {
            _blurDrawPass.blurStep = blurStep;

            _blurDrawPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(_blurDrawPass);
            renderer.EnqueuePass(_normalDrawPass);
        }

    }
}
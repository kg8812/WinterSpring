using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurDrawPass : ScriptableRenderPass {

    private static readonly string PASS_TAG = "GaussianblurRenderPass";

    private static readonly int PROPERTY_TEMPBUFFER_1 = Shader.PropertyToID("_TempBuffer_1"); // 임시렌더텍스처 변수명
    private static readonly int PROPERTY_TEMPBUFFER_2 = Shader.PropertyToID("_TempBuffer_2");

    public int blurStep = 8; // 가우시안 블러 반복 횟수

    private Material _material;
    private RenderTargetIdentifier _destination;  // 화면렌더텍스처(카메라)
    private RenderTargetIdentifier _tempBuffer_1 = new RenderTargetIdentifier(PROPERTY_TEMPBUFFER_1); // 임시렌더텍스처
    private RenderTargetIdentifier _tempBuffer_2 = new RenderTargetIdentifier(PROPERTY_TEMPBUFFER_2); // 임시렌더텍스처

    //생성자
    public BlurDrawPass(RenderPassEvent renderPassEvent, Material material) {
        this.renderPassEvent = renderPassEvent;
        _material = material;
    }

    public void Setup(RenderTargetIdentifier renderTargetDestination) {
        _destination = renderTargetDestination;
    }

    // Unity는 매 프레임마다 Execute 메서드를 실행합니다.
    // 이 방법을 사용하면 사용자 정의 렌더링 기능을 구현할 수 있습니다.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {


        // CommandBuffer 유형 개체를 만듭니다. 이 객체는 실행할 렌더링 명령(rendering commands to execute) 목록을 보유합니다.
        // CommandBufferPool.Get()은 새 명령 버퍼를 가져와서 이름을 지정합니다.
        // boilerplate part는 CommandBuffer 선언, ExecuteCommandBuffer,  CommandBufferPool.Release
        CommandBuffer cmd = CommandBufferPool.Get(PASS_TAG);

        // 임시렌더텍스처 생성
        CameraData cameraData = renderingData.cameraData;

        RenderTextureDescriptor descriptor = new RenderTextureDescriptor(cameraData.camera.scaledPixelWidth, cameraData.camera.scaledPixelHeight);
        cmd.GetTemporaryRT(PROPERTY_TEMPBUFFER_1, descriptor, FilterMode.Bilinear);
        cmd.GetTemporaryRT(PROPERTY_TEMPBUFFER_2, descriptor, FilterMode.Bilinear);

        // 2 Pass 가우스안블러 렌더링
        cmd.Blit(_destination, _tempBuffer_1, _material, 0);    // Horizontal
        cmd.Blit(_tempBuffer_1, _tempBuffer_2, _material, 1);   // Vertical
        for (int i = 1; i < blurStep; i++) {
            cmd.Blit(_tempBuffer_2, _tempBuffer_1, _material, 0);
            cmd.Blit(_tempBuffer_1, _tempBuffer_2, _material, 1);
        }

        // 임시렌더텍스처를 화면렌더텍스처에 복사
        cmd.Blit(_tempBuffer_2, _destination);


        context.ExecuteCommandBuffer(cmd);

        CommandBufferPool.Release(cmd);

    }

}
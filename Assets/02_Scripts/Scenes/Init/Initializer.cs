using System;
using System.Collections.Generic;
using Apis;
using chamwhy;
using Cysharp.Threading.Tasks;
using Default;
using Directing;
using Managers;
using Save.Schema;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class Initializer : SerializedMonoBehaviour
{
    public static string staticSceneName = "";
    [Title("테스트용")] [SerializeField] private string sceneName;

    [Title("프리로드")]
    [InfoBox("여기 등록되지 않은 애셋은 게임플레이 도중 동기 로드되어 프레임 스파이크를 만듭니다.")]
    [SerializeField]
    private PreloadManifest globalManifest;

    [InfoBox("씬 이름 → 그 씬 전용 애셋 목록. 로딩 화면이 씬 활성화 직전에 참조합니다.")] [SerializeField]
    private SceneManifestTable sceneManifests;

    private void Awake()
    {
        Screen.SetResolution(1920,1080,true);
    }

    // Unity 이벤트 함수이므로 async void로 둔다.
    // UniTask를 반환하면 Unity가 반환값을 버려 예외가 관측되지 않고 사라진다.
    private async void Start()
    {
        try
        {
            // Addressables 초기화와 프리로드를 끝낸 뒤에 데이터 로드와 첫 씬 진입을 진행한다.
            // 여기서 미리 올려두지 않으면 게임플레이 도중 동기 로드가 발생한다.
            var ct = this.GetCancellationTokenOnDestroy();
            ResourceUtil.RegisterSceneManifests(sceneManifests);

            await ResourceUtil.InitializeAsync(ct);
            await ResourceUtil.PreloadAsync(globalManifest, null, AssetLifetime.Global, ct);

            GameManager.Item.LoadItems();
            Map.instance.DataInit();

            DialogueDirector.alreadySpoke = new();
            GameManager.UI.CreateUI("UI_MainHud", UIType.Main);
            GameManager.UI.CreateUI("UI_MiniMap", UIType.Main);
            GameManager.Save.LoadExceptSlot();

            if (staticSceneName != "")
            {
                sceneName = staticSceneName;
            }

            staticSceneName = "";

            GameManager.Scene.SceneLoad(sceneName, false);
        }
        catch (OperationCanceledException)
        {
            // 초기화 도중 오브젝트가 파괴된 정상 경로. 조용히 종료한다.
        }
        catch (Exception e)
        {
            // async void에서 예외가 묻히지 않도록 강제로 출력한다.
            Debug.LogError($"[초기화 실패] 게임 로딩 중 치명적 에러 발생: {e.Message}\n{e.StackTrace}");
        }
    }

#if UNITY_EDITOR

    [MenuItem("AssetDataBase/DataClear")]
    #endif
    [Button(ButtonSizes.Large),GUIColor(0.8f,0,0)]
    public static void DataClear()
    {
        SaveManager.ClearDataFiles();
        Debug.Log("기존의 데이터가 삭제되었습니다.");
    }

#if UNITY_EDITOR
    [MenuItem("AssetDataBase/Preload Report")]
#endif
    [Button(ButtonSizes.Large)]
    public static void PreloadReport()
    {
        ResourceUtil.LogPreloadReport();
    }
}

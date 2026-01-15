using System.Collections.Generic;
using chamwhy;
using chamwhy.DataType;
using chamwhy.Managers;
using Save.Schema;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusMenu : SerializedMonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI lobbySoulText;
    public TextMeshProUGUI bodyValue;
    public TextMeshProUGUI spiritValue;
    public TextMeshProUGUI finesseValue;
    public TextMeshProUGUI hpValue;
    public TextMeshProUGUI defValue;
    public TextMeshProUGUI healValue;
    public TextMeshProUGUI lvlValue;
    public TextMeshProUGUI expValue;

    public Dictionary<PlayerType, Sprite> icons = new();
    public Dictionary<PlayerType, GameObject> portraits = new();
    public Image characterIcon;
    public Image activeIcon;
    public Image passiveIcon;

    private int _nameId;
    private void OnEnable()
    {
        Player player = GameManager.instance.Player;

        if (player == null) return;

        _nameId = player.playerType switch
        {
            PlayerType.Ine => 1010011,
            PlayerType.Jingburger => 1010012,
            PlayerType.Lilpa => 1010013,
            PlayerType.Jururu => 1010014,
            PlayerType.Gosegu => 1010015,
            PlayerType.Viichan => 1010016,
            _ => 1010815
        };

        nameText.text = LanguageManager.Str(_nameId);
        foreach (var keyValuePair in portraits)
        {
            keyValuePair.Value.SetActive(keyValuePair.Key == player.playerType);
        }

        characterIcon.sprite = icons[player.playerType];
        soulText.text = GameManager.instance.Soul.ToString();
        lobbySoulText.text = GameManager.instance.LobbySoul.ToString();
        bodyValue.text = player.Body.ToString();
        spiritValue.text = player.Spirit.ToString();
        finesseValue.text = player.Finesse.ToString();
        hpValue.text = player.MaxHp.ToString();
        defValue.text = player.Def.ToString();
        healValue.text = player.CalculateRepair().ToString();
        lvlValue.text = GameManager.instance.Level.ToString();
        expValue.text =
            $"다음 레벨까지 필요한 경험치 {GameManager.instance.Exp}/{LevelDatabase.GetLevelData(GameManager.instance.Level).exp}";
        if (player.ActiveSkill == null)
        {
            activeIcon.gameObject.SetActive(false);
        }
        else
        {
            activeIcon.gameObject.SetActive(true);
            activeIcon.sprite = player.ActiveSkill.SkillImage;
        }

        if (player.PassiveSkill == null)
        {
            passiveIcon.gameObject.SetActive(false);
        }
        else
        {
            passiveIcon.gameObject.SetActive(true);
            passiveIcon.sprite = player.PassiveSkill.SkillImage;
        }
    }
}
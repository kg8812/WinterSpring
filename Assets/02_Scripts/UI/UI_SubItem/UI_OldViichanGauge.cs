// using Apis;
// using Default;
// using UI;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UI_OldViichanGauge : UI_Base
// {
//     public enum Images
//     {
//         Gauge,LeafIcon,Frame
//     }
//
//     private Image gauge;
//     private Image leafIcon;
//     private Image gaugeBg;
//     private Image frame;
//     
//     private ViichanOldPassiveSkill skill;
//
//     public Sprite[] leafSprites;
//     public Sprite[] barSprites;
//     public Sprite[] frameSprites;
//     
//     public override void Init()
//     {
//         base.Init();
//         Bind<Image>(typeof(Images));
//         gauge = Get<Image>((int)Images.Gauge);
//         gaugeBg = GetComponent<Image>();
//         gameObject.SetActive(false);
//         frame = GetImage((int)Images.Frame);
//         UI_MainHud.Instance.setEvent.AddListener(x =>
//         {
//             if (x.PlayerPassiveSkill is ViichanOldPassiveSkill v)
//             {
//                 skill = v;
//                 gameObject.SetActive(true);
//                 skill.OnBlackKnight.AddListener(() =>
//                 {
//                     frame.color = new Color(1, 1, 1, 1);
//                     gaugeBg.sprite = barSprites[0];
//                     frame.sprite = frameSprites[0];
//                     leafIcon.sprite = leafSprites[2];
//                 });
//                 skill.OnAfterDuration.AddListener(() =>
//                 {
//                     frame.color = new Color(1, 1, 1, 0);
//                     gaugeBg.sprite = barSprites[2];
//                     leafIcon.sprite = leafSprites[0];
//                 });
//                 skill.OnHolyKnight.AddListener(() =>
//                 {
//                     frame.color = new Color(1, 1, 1, 1);
//                     gaugeBg.sprite = barSprites[1];
//                     frame.sprite = frameSprites[1];
//                     leafIcon.sprite = leafSprites[3];
//                 });
//             }
//             else
//             {
//                 skill = null;
//                 gameObject.SetActive(false);
//             }
//         });
//         leafIcon = Get<Image>((int)Images.LeafIcon);
//         gaugeBg.sprite = barSprites[2];
//         frame.color = new Color(1, 1, 1, 0);
//     }
//
//     private void Update()
//     {
//         if (ReferenceEquals(skill, null)) return;
//
//         gauge.fillAmount = Mathf.Clamp01(skill.CurGauge / skill.maxGauge);
//
//         if (skill.IsUse) return;
//         
//         leafIcon.sprite = Mathf.Approximately(gauge.fillAmount, 1) ? leafSprites[1] : leafSprites[0];
//     }
// }
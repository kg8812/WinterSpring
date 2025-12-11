// using UnityEngine;
// using UnityEngine.UI;
//
// namespace chamwhy.UI
// {
//     public class TabMenuButton: UIElement
//     { 
//         //리소스 확보 후 캐릭터 선택 버튼 기능 분할 후 새로 코드 작성해야 함
//         [SerializeField] private GameObject arrow;
//
//         [SerializeField]
//         Image IconImage;
//         // [SerializeField]
//         // TextMeshProUGUI text;
//         // protected override void Start()
//         // {
//         //     base.Start();
//         //     Material targetMat = Instantiate(targetGraphic.material);
//         //     targetGraphic.material = targetMat;
//         //     Material iconMat = Instantiate(IconImage.material);
//         //     IconImage.material = iconMat;
//         //
//         // }
//         //
//         // private void Update()
//         // {
//         //     if (IsHovered && !IsSelected)
//         //     {
//         //         targetGraphic.material.SetFloat("_UnScaledTime", Time.unscaledTime);
//         //     }
//         // }
//         //
//         //
//         // public override void HoverOn()
//         // {
//         //     base.HoverOn();
//         //     if (!IsSelected)
//         //     {
//         //         targetGraphic.gameObject.SetActive(true);
//         //         targetGraphic.material.SetInt("_IsHovering",1);
//         //     }
//         //     //if (selectedBySystem && !IsSelected) {
//         //     //    arrow.SetActive(true);
//         //     //    IconImage.material.SetInt("_IsSelect", 1);
//         //     //    text.color = Color.white;
//         //     //}
//         //         
//         // }
//         //
//         // public override void HoverOff()
//         // {
//         //     base.HoverOff();
//         //     if (!IsSelected)
//         //     {
//         //         targetGraphic.gameObject.SetActive(false);
//         //         targetGraphic.material.SetInt("_IsHovering",0);
//         //     }
//         //     //if (selectedBySystem && !IsSelected) {
//         //     //    arrow.SetActive(false);
//         //     //    IconImage.material.SetInt("_IsSelect", 0);
//         //     //    text.color = Color.gray;
//         //     //}
//         //
//         // }
//         //
//         // public override void Selected()
//         // {
//         //     base.Selected();
//         //     arrow.SetActive(true);
//         //     IconImage.material.SetInt("_IsSelect", 1);
//         //     text.color = Color.white;
//         //     targetGraphic.gameObject.SetActive(true);
//         //     targetGraphic.material.SetInt("_IsHovering", 0);
//         //
//         // }
//         //
//         // public override void UnSelected()
//         // {
//         //     base.UnSelected();
//         //     arrow.SetActive(false);
//         //     IconImage.material.SetInt("_IsSelect", 0);
//         //     text.color = Color.gray;
//         //     if (!IsHovered)
//         //        targetGraphic.gameObject.SetActive(false);
//         //     else
//         //         targetGraphic.material.SetInt("_IsHovering", 1);
//         // }
//     }
// }
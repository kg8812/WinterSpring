// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Apis
// {
//     public class InvenMenuWindow : MonoBehaviour
//     {
//         [SerializeField] List<Button> buttons;
//         int _index;
//         Button _selectedButton;
//
//         private void Awake()
//         {
//             _index = 0;
//
//             Select();
//         }
//         public void Init(AbsInvenMenu menu)
//         {
//             foreach (var button in buttons)
//             {
//                 button.onClick.RemoveAllListeners();
//                 button.onClick.AddListener(() => gameObject.SetActive(false));
//             }
//             menu.ApplyButtons(buttons);
//         }
//
//         public void OnEnter()
//         {
//             foreach (var button in buttons)
//             {
//                 button.image.enabled = false;
//             }
//
//             _index = 0;
//             Select();
//         }
//         public void MoveToTopButton()
//         {
//             _index--;
//             if (_index < 0) _index = buttons.Count - 1;
//
//             _index = Mathf.Clamp(_index, 0, buttons.Count - 1);
//
//             Select();
//         }
//
//         public void MoveToBottomButton()
//         {
//             _index++;
//             if (_index > buttons.Count - 1) _index = 0;
//
//             _index = Mathf.Clamp(_index, 0, buttons.Count - 1);
//             Select();
//
//         }
//
//         public void InvokeButton()
//         {
//             if (_selectedButton != null)
//             {
//                 _selectedButton.onClick.Invoke();
//             }
//         }
//
//         void Select()
//         {
//             if (_selectedButton != null) _selectedButton.image.enabled = false;
//             _selectedButton = buttons[_index];
//             buttons[_index].image.enabled = true;
//         }
//
//         public void Choose(int x)
//         {
//             _index = x;
//             Select();
//         }
//     }
//
//
// }

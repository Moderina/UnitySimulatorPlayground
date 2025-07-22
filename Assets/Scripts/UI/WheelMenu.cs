using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WheelMenu : MonoBehaviour
    {
        public static event Action<int> OnToolChanged;
        
        [SerializeField] private GameObject WheelMenuElementPrefab;
        [SerializeField] private GameObject WheelMenuContainer;
        
        [SerializeField] private List<WheelMenuElement> wheelMenuElements = new List<WheelMenuElement>();
        private int currentMenuElementIndex = 0;

        private float ElementSize;
        void Start()
        {
            Init();
            gameObject.SetActive(false);
        }
        
        void Init()
        {
            ElementSize = 360f / wheelMenuElements.Count;
            float currentElementRotation = 0;

            for (int i = 0; i < wheelMenuElements.Count; i++)
            {
                GameObject menuElement = GameObject.Instantiate(WheelMenuElementPrefab);
                menuElement.name = i.ToString();
                menuElement.transform.SetParent(WheelMenuContainer.transform);
                
                RectTransform rect = menuElement.GetComponent<RectTransform>();
                
                rect.localScale = Vector3.one;
                rect.localPosition = Vector3.zero;
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.rotation = Quaternion.Euler(0, 0, currentElementRotation);
                
                Image background = menuElement.GetComponent<Image>();
                background.fillAmount = 1f / wheelMenuElements.Count - 0.001f;
                
                Image image = menuElement.transform.GetChild(0).GetComponent<Image>();
                
                image.sprite = wheelMenuElements[i].Icon;

                float angleForIcon = ElementSize / 2;
                
                Vector3 pos = new Vector3(
                    -228 * MathF.Sin(Mathf.Deg2Rad * angleForIcon),
                    -228 * MathF.Cos(Mathf.Deg2Rad * angleForIcon), 
                    0);

                image.GetComponent<RectTransform>().localPosition = pos;
                image.GetComponent<RectTransform>().rotation = Quaternion.identity;
                
                currentElementRotation += ElementSize;

            }
        }

        void Update()
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            
            float cursorAngle = (90 + ElementSize + Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg + 360) % 360;
            // Debug.Log(cursorAngle);
            
            int MenuElementIndex = (int)(cursorAngle / ElementSize);

            if (MenuElementIndex != currentMenuElementIndex)
            {
                WheelMenuContainer.transform.GetChild(currentMenuElementIndex).GetComponent<RectTransform>().localScale = Vector3.one; 
                currentMenuElementIndex = MenuElementIndex;
                WheelMenuContainer.transform.GetChild(currentMenuElementIndex).GetComponent<RectTransform>().localScale = Vector3.one * 1.3f; 

            }
        }

        public void ChangeTool()
        {
            Debug.Log("selected tool: "  + currentMenuElementIndex);
            OnToolChanged?.Invoke(currentMenuElementIndex-1);
            //Event: onToolChanged
        }
    }
}
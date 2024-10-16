using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GH
{
    public class InventoryUI : MonoBehaviour
    {
        private GraphicRaycaster inventory_GraphicRay;
        private PointerEventData inventory_PointerEventData;
        private List<RaycastResult> inventory_RayResultList;

        // 현재 드래그를 시작한 슬롯
        private ItemSlotUI beginDragSlot;
        // 해당 스롯의 아이콘 트랜스폼
        private Transform beginDragIcon_Transform;

        //드래그 시작 시 슬롯의 위치
        private Vector3 beginDragIconPoint;
        //드래그 시작 시 커서의 위치
        private Vector3 beginDragCursorPoint;
        private int beginDragSlotSiblingIndex;

        
        private void Update()
        {
            //컴퓨터 조작
            inventory_PointerEventData.position = Input.mousePosition;

            OnPointDown();
            OnPointDrage();
            OnPointUp();
        }

        private T RayCastGetFirstComponent<T>() where T : Component
        {
            inventory_RayResultList.Clear();

            inventory_GraphicRay.Raycast(inventory_PointerEventData, inventory_RayResultList);

            if(inventory_RayResultList.Count == 0)
            {
                return null;
            }
            return inventory_RayResultList[0].gameObject.GetComponent<T>();
       
        }

        private void OnPointDown()
        {

        }
        
        private void OnPointDrage()
        {

        }
         
        private void OnPointUp()
        {

        }
    }

}
﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace R1Engine.Unity {
    public class EventListItem : MonoBehaviour, IPointerDownHandler {
        public Common_Event ev;
        public Text evName;
        EventList list;
        Image bg;

        void Start() {
            list = GetComponentInParent<EventList>();
            bg = GetComponent<Image>();
            evName.text = ev.EventInfoData.Type.ToString();
        }

        public void OnPointerDown(PointerEventData eventData) {
            list.selection = ev;
        }

        void Update() {
            bg.enabled = list.selection == ev;
        }
    }
}
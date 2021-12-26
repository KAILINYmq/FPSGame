using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    public class BaseItem : MonoBehaviour
    {
        public enum ItemType 
        {
            Firearms,
            Attachment,
            Grenade,
            knife, 
            Others
        }

        public ItemType CurrentItemType;
        public int ItemId;
        public string ItemName;
    }
}

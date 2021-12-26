using System.Collections;
using System.Collections.Generic;
using Scripts.Items;
using UnityEditor;
using UnityEngine;

public class AttachmentItem : BaseItem
{
        public enum AttachmentType
        { 
                Scope,
                Other
        }

        public AttachmentType CurrentAttachmentType;
}

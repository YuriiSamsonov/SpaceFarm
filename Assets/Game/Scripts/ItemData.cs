using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "ItemData", fileName = "new ItemData")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField]
        public string ID;
        
        [field: SerializeField]
        public Sprite ItemSprite;
        
        [field: SerializeField] 
        public ItemType Type;
        
        [ContextMenu("Update ID")]
        private void UpdateID()
        {
            var guid = Guid.NewGuid();
            var newID = Convert.ToBase64String(guid.ToByteArray());
            newID = Regex.Replace(newID, "[^a-zA-Z0-9]", "");
            ID = newID.Substring(0, Math.Min(15, newID.Length));
        }
    }

    public enum ItemType
    {
        Plant,
        Meat
    }
}
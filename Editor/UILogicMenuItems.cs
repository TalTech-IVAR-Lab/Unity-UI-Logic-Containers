using UnityEditor;
using UnityEngine;

namespace EE.TalTech.IVAR.UnityUILogicContainers.Editor
{
    internal static class UILogicMenuItems
    {
        private const string LogicContainerMenuName = "GameObject/UI/Logic Container";

        [MenuItem(LogicContainerMenuName, false, 10)]
        private static void AddLogicContainer()
        {
            var selectedRect = Selection.activeTransform.GetComponent<RectTransform>();
            if (selectedRect == null) return;

            UILogicContainers.CreateLogicContainerUnderRectTransform(selectedRect);
        }

        [MenuItem(LogicContainerMenuName, true)]
        private static bool ValidateAddLogicContainer()
        {
            var activeTransform = Selection.activeTransform;
            
            if (activeTransform == null) return false;
            if (activeTransform.GetComponent<RectTransform>() == null) return false;
            if (PrefabUtility.IsPartOfAnyPrefab(activeTransform)) return false;
            
            return true;
        }
    }
}
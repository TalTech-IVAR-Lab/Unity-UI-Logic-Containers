using UnityEngine;
using UnityEngine.Serialization;

namespace EE.TalTech.IVAR.UnityUILogicContainers
{
    public class UILogicContainer : MonoBehaviour
    {
        [HideInInspector] public RectTransform linkedParentRect;
        [FormerlySerializedAs("linkedActionsContainer")] [HideInInspector] public UIActionsContainer linkedUIActionsContainer;
    }
}
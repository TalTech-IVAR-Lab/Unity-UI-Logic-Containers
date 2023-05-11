using UnityEditor;
using UnityEngine;
using Zinnia.Event.Proxy;

namespace EE.TalTech.IVAR.UnityUILogicContainers.Editor
{
    [InitializeOnLoad]
    internal static class UILogicContainers
    {
        private const string LogicContainerName = "✦ Logic";
        private const string ActionsContainerName = "✦ Actions";

        #region Lifecycle

        static UILogicContainers()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPaused)
            {
                // pnly lint in Edit mode
                return;
            }

            LintLogicContainers();
        }

        private static void LintLogicContainers()
        {
            var logicContainers = Object.FindObjectsOfType<UILogicContainer>();

            foreach (var logicContainer in logicContainers)
            {
                // keep logic container at the top of its siblings
                logicContainer.transform.SetAsFirstSibling();
                if (logicContainer.transform.parent != logicContainer.linkedParentRect.transform)
                {
                    // keep logic container parented to its original rect at all times
                    GameObjectUtility.SetParentAndAlign(logicContainer.gameObject,
                        logicContainer.linkedParentRect.gameObject);

                    Debug.LogWarning("Logic container cannot be detached from its original parent rect.",
                        logicContainer);
                }

                // keep actions container at the top of its siblings
                logicContainer.linkedUIActionsContainer.transform.SetAsFirstSibling();
                if (logicContainer.linkedUIActionsContainer.transform.parent != logicContainer.transform)
                {
                    // keep actions container parented to logic container at all times
                    GameObjectUtility.SetParentAndAlign(logicContainer.linkedUIActionsContainer.gameObject,
                        logicContainer.gameObject);

                    Debug.LogWarning("Actions container cannot be detached from its original logic container.",
                        logicContainer.linkedUIActionsContainer);
                }

                // enforce naming
                logicContainer.gameObject.name = LogicContainerName;
                logicContainer.linkedUIActionsContainer.gameObject.name = ActionsContainerName;
            }
        }

        #endregion

        #region Management

        public static void CreateLogicContainerUnderRectTransform(RectTransform rect)
        {
            GameObject CreateChild(string name, GameObject parent)
            {
                var child = new GameObject(name);
                GameObjectUtility.SetParentAndAlign(child, parent);
                child.isStatic = true;
                child.transform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
                child.transform.SetAsFirstSibling();

                return child;
            }

            // create containers
            var logicContainer = CreateChild(LogicContainerName, rect.gameObject).AddComponent<UILogicContainer>();
            var actionsContainer = CreateChild(ActionsContainerName, logicContainer.gameObject)
                .AddComponent<UIActionsContainer>();

            // set hide flags
            logicContainer.hideFlags = HideFlags.NotEditable;
            actionsContainer.hideFlags = HideFlags.NotEditable;

            // set links
            logicContainer.linkedParentRect = rect;
            logicContainer.linkedUIActionsContainer = actionsContainer;

            // create new action and select it for user convenience
            AddNewAction(logicContainer);

            Undo.RegisterCreatedObjectUndo(logicContainer, "Create Logic Container");
        }

        public static void AddNewAction(UILogicContainer container, string actionName = "New Action")
        {
            var newAction = new GameObject(actionName);
            GameObjectUtility.SetParentAndAlign(newAction, container.linkedUIActionsContainer.gameObject);
            newAction.AddComponent<EmptyEventProxyEmitter>();
            newAction.hideFlags = HideFlags.None;
            Selection.activeGameObject = newAction;
        }

        #endregion
    }
}
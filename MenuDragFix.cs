using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ABI_RC.Core.InteractionSystem;
using cohtml.Net;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace MenuDragFix
{
    public static class BuildInfo
    {
        public const string Name = "MenuDragFix";
        public const string Author = "DDAkebono#0001";
        public const string Company = "BTK-Development";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/ddakebono/MenuDragFix/releases";
    }
    
    public class MenuDragFix : MelonMod
    {
        internal static MelonLogger.Instance Logger;
        internal static MelonPreferences_Entry<bool> EnableDragFix;

        public override void OnInitializeMelon()
        {
            Logger = LoggerInstance;

            Logger.Msg("MenuDragFix - Start up");

            if (RegisteredMelons.Any(x => x.Info.Name.Equals("BTKCompanionLoader", StringComparison.OrdinalIgnoreCase)))
            {
                Logger.Msg("Hold on a sec! Looks like you've got BTKCompanion installed, this mod is built in and not needed!");
                Logger.Error("MenuDragFix has not started up! (BTKCompanion Running)");
                return;
            }

            EnableDragFix = MelonPreferences.CreateEntry("MenuDragFix", "EnableDragFix", true, "Enable Menu Drag Fix", "Enables the menu drag fix patch, can be toggled on the fly");
            ApplyPatches(typeof(ControllerRayPatches));
        }
        
        private void ApplyPatches(Type type)
        {
            try
            {
                HarmonyInstance.PatchAll(type);
            }
            catch (Exception e)
            {
                Logger.Error($"Failed while patching {type.Name}!");
                Logger.Error(e);
            }
        }
    }
    
    [HarmonyPatch(typeof(ControllerRay))]
    class ControllerRayPatches
    {
        private static readonly MethodInfo MouseEvent = typeof(View).GetMethod(nameof(View.MouseEvent), BindingFlags.Instance | BindingFlags.Public);
        private static readonly MethodInfo DragFixMethod = typeof(ControllerRayPatches).GetMethod(nameof(DragFix), BindingFlags.Static | BindingFlags.NonPublic);

        private static bool _mouseDown;
        private static float _deltaTimeSincePress = 0f;
        
        [HarmonyPatch("LateUpdate")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> LateUpdateFiddler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var rayFiddler = codeInstructions as CodeInstruction[] ?? codeInstructions.ToArray();
            
            for(int i=0; i < rayFiddler.Length; i++)
            {
                var instruction = rayFiddler[i];

                if (instruction.opcode == OpCodes.Callvirt && ReferenceEquals(instruction.operand, MouseEvent))
                {
                    //Time to fiddle shit!
                    rayFiddler[i] = new CodeInstruction(OpCodes.Callvirt, DragFixMethod);
                }
            }

            return rayFiddler;
        }

        private static void DragFix(View view, MouseEventData data)
        {
            if (!MenuDragFix.EnableDragFix.Value)
            {
                view.MouseEvent(data);
                return;
            }

            if (data.Type == MouseEventData.EventType.MouseDown)
            {
                _mouseDown = true;
                _deltaTimeSincePress = 0f;
            }

            if (data.Type == MouseEventData.EventType.MouseUp)
            {
                _mouseDown = false;
            }

            if (_mouseDown)
                _deltaTimeSincePress += Time.deltaTime;

            //Check if we should wait before allowing the movement to continue (dragfix)
            if (data.Type == MouseEventData.EventType.MouseMove && _mouseDown && _deltaTimeSincePress <= .150f)
            {
                return;
            }

            //Call the original MouseEvent
            view.MouseEvent(data);
        }
    }
}
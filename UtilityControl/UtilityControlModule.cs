
using Celeste.Mod.UI;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;
using Celeste;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using IL.Celeste.Mod;
//using Logger = IL.Celeste.Mod.Logger;

namespace Celeste.Mod.UtilityControl
{
    public class UtilityControlModule : EverestModule
    {
        protected const string UC_ModuleName = "UtilityControl";
        //Celeste.Mod.EverestModule
        AreaMode mAreaMode;
        Boolean mAreaModeOverride;
        MTexture mDeathIcon;
        public static SpriteBank spriteBank;
        MTexture _DeathIconA;
        MTexture _DeathIconB;
        MTexture _DeathIconC;




        // Only one alive module instance can exist at any given time.
        public static UtilityControlModule Instance;

        public UtilityControlModule()
        {
            Instance = this;
        }

        // If you don't need to store any settings, => null
        //public override Type SettingsType => null;
        public override Type SettingsType => typeof(UtilityControlSettings);
        public static UtilityControlSettings Settings => (UtilityControlSettings)Instance._Settings;

        // If you don't need to store any save data, => null
        public override Type SaveDataType => null;
        public List<MTexture> textures = null;
        // If you don't need to store any save data, => null
        //public override Type SaveDataType => typeof(ExampleSaveData);
        //public static ExampleSaveData SaveData => (ExampleSaveData)Instance._SaveData;



        // Set up any hooks, event handlers and your mod in general here.
        // Load runs before Celeste itself has initialized properly.
        public override void Load()
        {
            Everest.Events.Level.OnLoadLevel += new Everest.Events.Level.LoadLevelHandler(this.OnLoadLevel);
            Everest.Events.Level.OnLoadEntity += new Everest.Events.Level.LoadEntityHandler(this.OnLoadEntity);
            //Everest.Events.Level.OnPause
            //Everest.Events.Player.
            
            Logger.Log(LogLevel.Info, UC_ModuleName, "Loading Utility Control");
            mAreaMode = AreaMode.Normal;

            /*
            _DeathIconA = GFX.Gui["collectables/skullBlue"];
            _DeathIconB = GFX.Gui["collectables/skullRed"];
            _DeathIconC = GFX.Gui["collectables/skullGold"];
            */
        }


        // Optional, initialize anything after Celeste has initialized itself properly.
        public override void Initialize()
        {
            Logger.Log(LogLevel.Info, UC_ModuleName, "Initialising UtilityControl.");
        }

        // Optional, do anything requiring either the Celeste or mod content here.
        public override void LoadContent(bool firstLoad){
            spriteBank = new SpriteBank(GFX.Game, "Graphics/UtilCSprites.xml");
        }

        // Unload the entirety of your mod's content, remove any event listeners and undo all hooks.
        public override void Unload()
        {
            Everest.Events.Level.OnLoadEntity -= new Everest.Events.Level.LoadEntityHandler(this.OnLoadEntity);
            Logger.Log(LogLevel.Info, UC_ModuleName, "Unloading UtilityControl.");
        }

        public bool OnLoadEntity(Level level, LevelData levelData, Vector2 offset, EntityData entityData)
        {
            ///TODO: Reedit section here
            //Logger.Log("UtilControl Debug...", entityData.ToString());

            //bool flag = ;
            string header = "utilitycontrol/";
            bool result ;
            if (!entityData.Name.StartsWith(header))
            {
                result = false;
            }
            else
            {
                int esketit = header.Length;
                string name = entityData.Name.Substring(esketit);
                //if (!(a == "movingtouchswitch"))
                if (!(name == "popblock"))
                {
                    if (!(name == "popblockdetonator"))
                    {
                        result = false;
                    }
                    else
                    {
                        level.Add(new PopBlock(entityData, offset));
                        //level.Add(new PopBlockDetonator(entityData, offset);
                        result = true;
                    }
                }
                else
                {
                    int width = entityData.Width;
                    int height = entityData.Height;

                    Logger.Log(LogLevel.Debug, "UtilControl Popblock Check", "PopBlock Stack -- Width: " + width + " - Height: " + height + " ");
                    Vector2 pos = entityData.Position + offset;
                    Vector2 offsetNew = offset;
                    for(int i = 0; i < width / 8; ++i)
                    {
                        for (int j = 0; j < height / 8; ++j)
                        {
                            offsetNew.X = i * 8f;
                            offsetNew.Y = j * 8f;
                            //offsetNew = offset + new Vector2()
                            level.Add(new PopBlock(entityData, offset + offsetNew));
                        }
                    }
                    //level.Add(new PopBlock(entityData, offset));
                    result = true;
                }
            }
            return result;
        }

        public void OnLoadLevel(Level level, Player.IntroTypes playerIntro, bool isFromLoader)
        {
            mAreaMode = level.Session.Area.Mode;
            Logger.Log(LogLevel.Info, UC_ModuleName, "LevelLoad: Area (" + DeathModeToString(mAreaMode) +") for DeathCounter.");
            SetDeathIconMode(mAreaMode);
            Settings.AreaDeathMode = DeathModeToString(this.mAreaMode);
        }


        /*
        public override void CreateModMenuSection(TextMenu menu, bool inGame, FMOD.Studio.EventInstance snapshot)
        {
            base.CreateModMenuSection(menu, inGame, snapshot);
            //menu.Add(new TextMenu.SubHeader())
            //menu.Add(new TextMenu.SubHeader(Dialog.Clean("modoptions_ghostmodule_overridden") + " | v." + Metadata.VersionString));            
        }
        */

        public static void SetDeathModeOverride(Boolean ovr)
        {
            Instance.mAreaModeOverride = ovr;
        }
        //Set the death icon to appear in menu
        public void SetDeathIconMode(AreaMode mode)
        {

            if (mode == AreaMode.Normal)
            {
                mDeathIcon = GFX.Gui["collectables/skullBlue"];
            }
            else
            {
                if (mode == AreaMode.BSide)
                {
                    mDeathIcon = GFX.Gui["collectables/skullRed"];
                }
                else
                {
                    mDeathIcon = GFX.Gui["collectables/skullGold"];
                }
            }
        }
        protected static string DeathModeToString(AreaMode mode)
        {
            string s = "";
            if (mode == AreaMode.Normal){
                s = "Normal";
            }
            else{
                if (mode == AreaMode.BSide){
                    s = "BSide";
                }
                else{
                    if(mode == AreaMode.CSide)
                    {
                        s = "CSide";
                    }
                    else { s = "Undefined-Default-AreaMode"; }
                }
            }
            //return s;
            return Dialog.Clean(s);
        }
        public static string DeathMode
        {
            get
            {
                if (Instance != null)
                {
                    return DeathModeToString(Instance.mAreaMode);
                }
                else
                {
                    return "Default";
                }
            }
        }
    }
}


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


namespace Celeste.Mod.UtilityControl
{
    public class UtilityControlModule : EverestModule
    {

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
        public static SpriteBank spriteBank;
        // Set up any hooks, event handlers and your mod in general here.
        // Load runs before Celeste itself has initialized properly.
        public override void Load()
        {
            Everest.Events.Level.OnLoadEntity += new Everest.Events.Level.LoadEntityHandler(this.OnLoadEntity);
            Logger.Log(LogLevel.Info, "UtilControl", "Loading Utility Control");
            
        }

        // Optional, initialize anything after Celeste has initialized itself properly.
        public override void Initialize()
        {
            Logger.Log(LogLevel.Info, "UtilControl", "Initialising Utility Control.");

        }

        // Optional, do anything requiring either the Celeste or mod content here.
        public override void LoadContent(bool firstLoad){
            
            spriteBank = new SpriteBank(GFX.Game, "Graphics/UtilCSprites.xml");
        }

        // Unload the entirety of your mod's content, remove any event listeners and undo all hooks.
        public override void Unload()
        {
            Everest.Events.Level.OnLoadEntity -= new Everest.Events.Level.LoadEntityHandler(this.OnLoadEntity);
            Logger.Log(LogLevel.Info, "UtilControl", "Unloading Utility Control.");
            
        }

        public bool OnLoadEntity(Level level, LevelData levelData, Vector2 offset, EntityData entityData)
        {
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
                string a = name;
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

    }
}

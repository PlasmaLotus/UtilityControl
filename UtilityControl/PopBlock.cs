﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celeste.Mod.UI;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Celeste;
using YamlDotNet;
using Monocle;
using System.Collections;

namespace Celeste.Mod.UtilityControl
{
   public class PopBlock: Solid
    {
        private Vector2 start;
        private string startLevel;
        private float startDisappearTime;
        private float currentDisappearTime;
        private PlayerCollider collider;


        private Image popblock;
        private Image outline;

        private Color starterColor = new Color(0.77f, 0.91f, 0.27f);
        private List<Solid> PopList;
        private bool emittedFire = false;
        
        private float bounceSFXDelay;
        private Wiggler moveWiggler;
        private Vector2 moveWigglerDir;
        private bool activated;
        private float activationCooldown;
        private Coroutine outlineFader;
        private List<Image> images;
        private Image image;
        private PopColor popColor;
        private Color color;
        private enum DisappearTimes
        {
            Fast = 5,
            Medium = 10,
            Slow = 15
        }
        public enum PopColor
        {
            Purple,
            Blue,
            Red,
            Yellow,
            Green,
            Pink
        }
        private Color[] popColorArray = new Color[]
        {
            new Color(1f, 0.3f, 1f),
            new Color(0.3f, 0.3f, 1f),
            new Color(1f, 0.3f, 0.3f),
            new Color(1f, 1f, 0.3f),
            new Color(0.3f, 1f, 0.3f),
            new Color(1f, 0.5f, 0.8f)
        };

        private Level level
        {
            get
            {
                return (Level)base.Scene;
            }
        }

        public PopBlock(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Enum<PopColor>("color", PopColor.Pink))
        {
            //this.popColor =;
            //this.popColor = data.Enum<PopColor>("color", PopColor.Pink);
            //this.color = popColorArray[(int)this.popColor];
        }

        public PopBlock(Vector2 position, PopColor color):
            base(position, 8f, 8f,false)
        {
            //Color            
            //this.popColor = data.Enum<PopColor>("color", PopColor.Pink);
            this.popColor = color;
            this.color = popColorArray[(int)this.popColor];
            //this

            //this.startDisappearTime = (float)data.Enum<TimedTouchSwitch.DisappearTimes>("startDisappearTime", TimedTouchSwitch.DisappearTimes.Slow);
            //this.image = base.Get<Sprite>();

            /*
            this.collider = base.Get<PlayerCollider>();
            this.collider.OnCollide = new Action<Player>(this.OnPlayer);
           
            this.collider = new PlayerCollider(new Action<Player>(this.OnPlayer));
            base.Add(this.collider);
            */

            this.moveWiggler = Wiggler.Create(0.8f, 2f, null, false, false);
            base.Add(this.moveWiggler);


            this.activationCooldown = 3f;
            this.activated = false;
            //DashCollision d = new DashCollision()
        }

         

        public override void Added(Scene scene)
        {

            ///Check for all other PopBlocks
            base.Added(scene);
            //this.level = base.SceneAs<Level>();
            //Level level = scene as Level;
            //MTexture mTexture = GFX.Game["objects/crumbleBlock/outline"];
            MTexture mTexture = GFX.Game["objects/utilitycontrol/popblockoutline"];

            /*Outline*/
            this.outline = new Image(mTexture.GetSubtexture(0, 0, 8, 8, null));
            //this.outline.Color = this.color;
            base.Add(this.outline);
     
            base.Add(this.outlineFader = new Coroutine(true));
            this.outlineFader.RemoveOnComplete = false;

            /*Sprite*/
            //this.images = new List<Image>();
            MTexture mTexture2 = GFX.Game["utilitycontrol/popblock"];
            this.popblock = new Image(mTexture2.GetSubtexture(0, 0, 8, 8, null));
            //this.image = new Image(mTexture2.GetSubtexture(8, 0, 8, 8, null);
            //this.popblock.Color = this.color;
            base.Add(this.popblock);
            


        }

        public override void Awake(Scene scene)
        {
            this.popblock.Color = this.color;
            this.outline.Color = this.color;
            //this.PopList = scene.Entities.OfType<Solid>().ToList<Solid>();
            base.Awake(scene);
        }


        private void OnPlayer(Player player)
        {

            if (this.Active && !level.Frozen)
            {
                if (player.DashAttacking)
                {
                    this.activated = true;
                    //Start PopCoroutine

                }
            }
            else
            {
                //bounceOff
                if(this.bounceSFXDelay <= 0f)
                {
                    Audio.Play("event:/game/general/crystalheart_bounce", this.Position);
                    this.bounceSFXDelay = 0.1f;
                }
                player.PointBounce(base.Center);
                this.moveWiggler.Start();
                this.moveWigglerDir = (base.Center - player.Center).SafeNormalize(Vector2.UnitY);
                Input.Rumble(RumbleStrength.Light, RumbleLength.Medium);
            }
            
        }

        public override void Update()
        {
            base.Update();
            this.bounceSFXDelay -= Engine.DeltaTime;
            if (this.activated)
            {
                this.activationCooldown -= Engine.DeltaTime;
                if(this.activationCooldown <= 0f)
                {
                    this.activated = false;
                    //Start coroutine here i guess
                }
            }
            //this.sprite.Position = Vector2.UnitY * (float)Math.Sin((double)(this.timer * 2f)) * 2f + this.moveWiggleDir * this.moveWiggler.Value * -8f;
            
            float time = Engine.DeltaTime;
        }

        private IEnumerator OutlineFade(float to)
        {
            float from = 1f - to;
            for (float t = 0f; t < 1f; t += Engine.DeltaTime * 2f)
            {
                Color color = Color.White * (from + (to - from) * Ease.CubeInOut(t));
                this.outline.Color = color;
                /*
                foreach (Image img in this.outline)
                {
                    img.Color = color;
                    //img = null;
                }
                */
                //List<Image>.Enumerator enumerator = default(List<Image>.Enumerator);
                yield return null;
                color = default(Color);
            }
            yield break;
        }
    }
}

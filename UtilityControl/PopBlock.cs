using System;
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
        private Sprite icon;

        private Color starterColor = new Color(0.77f, 0.91f, 0.27f);
        private List<Solid> PopList;
        private bool emittedFire = false;
        private List<Image> outline;
        private float bounceSFXDelay;
        private Wiggler moveWiggler;
        private Vector2 moveWigglerDir;
        private bool activated;
        private float activationCooldown;
        private Coroutine outlineFader;
        private List<Image> images;
        private Image image;

        private enum DisappearTimes
        {
            Fast = 5,
            Medium = 10,
            Slow = 15
        }
        private enum PopColor
        {

        }

        private Level level
        {
            get
            {
                return (Level)base.Scene;
            }
        }

        //public PopBlock(Entity)

        public PopBlock(EntityData data, Vector2 offset) : 
            this(data.Position + offset)
        { }

        public PopBlock(Vector2 position):
            base(position, 8f, 8f,false)
        {
            //this.startDisappearTime = (float)data.Enum<TimedTouchSwitch.DisappearTimes>("startDisappearTime", TimedTouchSwitch.DisappearTimes.Slow);
            this.icon = base.Get<Sprite>();

            this.collider = base.Get<PlayerCollider>();
            this.collider.OnCollide = new Action<Player>(this.OnPlayer);
            base.Add(this.collider);

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
            MTexture mTexture = GFX.Game["objects/crumbleBlock/outline"];
            this.outline = new List<Image>();
            //if (base.Width <= 8f)
            {
                Image image = new Image(mTexture.GetSubtexture(24, 0, 8, 8, null));
                image.Color = Color.White * 0f;
                base.Add(image);
                this.outline.Add(image);
            }
            
            base.Add(this.outlineFader = new Coroutine(true));
            this.outlineFader.RemoveOnComplete = false;

            /*Sprite*/
            //this.images = new List<Image>();
            MTexture mTexture2 = GFX.Game["objects/crumbleBlock/" + AreaData.Get(scene).CrumbleBlock];
            this.image = new Image(mTexture2.GetSubtexture(8, 0, 8, 8, null);
            base.Add(this.image);
            //base.Add(new Coroutine(this.Sequence(), true));
            



        }

        public override void Awake(Scene scene)
        {

            this.PopList = scene.Entities.OfType<Solid>().ToList<Solid>();
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
                foreach (Image img in this.outline)
                {
                    img.Color = color;
                    //img = null;
                }
                List<Image>.Enumerator enumerator = default(List<Image>.Enumerator);
                yield return null;
                color = default(Color);
            }
            yield break;
        }
    }
}

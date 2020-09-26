using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
namespace Guardian
{
    //add in enemy shooting sounds, player hit sounds, a sound for out of ammo, and a sound for reloading --get a new backgroud music
    class Sound
    {
        public static Song background;  //background music 
        public static SoundEffect pShoot;
        public static SoundEffect ehit;
        public static SoundEffect t1Shoot;
        public static SoundEffect t2Shoot;
        public static SoundEffect pHit;
        public static SoundEffect Charge;
        public static SoundEffect noCharge;
        public Sound()
        {

        }
        public static void LoadContent(ContentManager cm)
        {
            background = cm.Load<Song>("music");
            pShoot = cm.Load<SoundEffect>("playerShoot");
            ehit = cm.Load<SoundEffect>("ehit");
            t1Shoot = cm.Load<SoundEffect>("t1shoot");
            t2Shoot = cm.Load<SoundEffect>("t2shoot");
            pHit = cm.Load<SoundEffect>("pHit");
            Charge = cm.Load<SoundEffect>("loaded");
            noCharge = cm.Load<SoundEffect>("noCharge");
            /*T1hit = cm.Load<Song>("t1hit");
            T2hit = cm.Load<Song>("t2hit");*/
        }
    }
}

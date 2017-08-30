using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Core;
using VeilofDeath.Objects.Traps;

namespace VeilofDeath.Objects
{
    class TrapHandler
    {
        private List<SpikeTrap> SPIKES;
        private List<SlowTrap> SLOWS;

        private Random rnd;

        public TrapHandler()
        {
            SPIKES = GameManager.Instance.getSpikeList();
            SLOWS = GameManager.Instance.getSlowList();
            rnd = new Random(1337);
        }


        public void choseTraps(GameTime time)
        {

            foreach (SpikeTrap spike in SPIKES)
            {
                if (rnd.NextDouble() < 0.05f)
                {
                    //Console.WriteLine("FLOAT!");
                    spike.isFloating = true;
                    spike.fStartFloating = (float)time.ElapsedGameTime.TotalSeconds;
                }                     
                spike.Tick(time);
            }
            
        }



 

    }
}

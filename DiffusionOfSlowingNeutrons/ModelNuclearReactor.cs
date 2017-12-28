using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuclearProject
{
    class ModelNuclearReactor
    {
        float Ct, pT, Vt, C, P, V, alphaFT, t0;

        public ModelNuclearReactor(
            float Ct,
            float pT,
            float Vt,
            float C,
            float P,
            float V,
            float alphaFT,
            float t0)
        {
            this.Ct = Ct;
            this.pT = pT;
            this.Vt = Vt;
            this.C = C;
            this.P = P;
            this.V = V;
            this.alphaFT = alphaFT;
            this.t0 = t0;
        }

        public float getCt() { return this.Ct; }
        public float getPt() { return this.pT; }
        public float getVt() { return this.Vt; }
        public float getC() { return this.C; }
        public float getP() { return this.P; }
        public float getV() { return this.V; }
        public float getAlphaFT() { return this.alphaFT; }
        public float getT0() { return this.t0; }

    }
}

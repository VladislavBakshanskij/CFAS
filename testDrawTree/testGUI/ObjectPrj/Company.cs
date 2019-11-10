using System;

namespace testGUI.ObjectPrj {
    public class Company {
        public static float CountProduct;
        public float[] changes = new float[3];
        public static float[] Prices = new float[3];

        public bool CheckChance {
            get {
                float sum = 0;
                foreach (float item in changes) sum += item;
                return Math.Floor(sum) >= 0.99f;
            }
        }

        public Company() : this(300, new float[] { 0.22f, 0.36f, 0.42f }) {
        }

        public Company(float countProduct, float[] changes) {
            CountProduct = countProduct;
            this.changes = changes;
        }

        public override string ToString() {
            return base.ToString();
        }
    }
}
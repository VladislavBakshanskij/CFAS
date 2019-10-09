namespace TestVersion {
    public class Company {
        public float countProduct;
        public float[] changes;//вероятность
        public float minPrice;
        public float srPrice;
        public float maxPrice;

        public bool WTF {
            get {
                float temp = 0;
                foreach (var item in changes) temp += item;
                return temp == 1;
            }
        }

        public Company()
            : this(300,  
                new float[]{
                    0.22f, 0.36f, 0.42f
                }, 
                1.51f, 1.8f, 2) {
        }

        public Company(float countProduct, float[] changes, float minPrice, float srPrice, float maxPrice) {
            this.countProduct = countProduct;
            this.changes = changes;
            this.minPrice = minPrice;
            this.srPrice = srPrice;
            this.maxPrice = maxPrice;
        }
    }
}
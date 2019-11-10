namespace testGUI.OldObject {
    public class Bank {
        public float precent;
        public float dateAvr;//срок взятия кредита
        public float money;

        public Bank()
            : this(28, 3, 500) {
        }

        public Bank(float procent, float dateAvr, float money) {
            this.precent = procent;
            this.dateAvr = dateAvr;
            this.money = money;
        }

        public override string ToString() {
            return $"Сумма взятая в кредит {money}\nСрок выплаты кредита {dateAvr}\nГодовой процент {precent}";
        }
    }
}
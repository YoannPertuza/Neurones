namespace Neurones
{
    public class Substr : Number
    {
        public Substr(Number number1, double number2) : this(number1, new DefaultNumber(number2))
        {
        }

        public Substr(double number1, Number number2): this(new DefaultNumber(number1), number2 )
        {
        }

        public Substr(Number number1, Number number2)
		{
            this.number1 = number1;
            this.number2 = number2;
		}

        private Number number1;
        private Number number2;

		public double value()
		{
			return this.number1.value() - this.number2.value();
		}
    }

}

namespace Neurones
{
    public class ExitError : Error
    {
        public ExitError(int index, Number number)
        {
            this.index = index;
            this.number = number;
        }

        private Number number;
        private int index;

        public Number asNumber()
        {
            return this.number;
        }

        public int neuroneIndex()
        {
            return this.index;
        }
    }

}

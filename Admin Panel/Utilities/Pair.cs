namespace Admin_Panel.Utilities
{
    public class Pair<T1, T2>
    {
        public Pair()
        {
            
        }
        public Pair(T1 First, T2 Second)
        {
            this.First = First;
            this.Second = Second;
        }
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }
}

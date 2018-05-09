namespace SlingRun
{
    public class Tuple<T1,T2>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;
        
        public Tuple(T1 a, T2 b)
        {
            Item1 = a;
            Item2 = b;
        }
    }
}
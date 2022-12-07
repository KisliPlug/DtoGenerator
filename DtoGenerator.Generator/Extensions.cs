namespace DtoGenerator.Generator;

public static class Extensions
{
    public static T1 PipeO<T, T1>(this T e, Func<T, T1> fu)
    {
        return fu(e);
    }

    public static T PipeAct<T>(this T e, Action<T> fu)
    {
          fu(e);
          return e;

    }
}

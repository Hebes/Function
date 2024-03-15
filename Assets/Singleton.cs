public class Singleton<T> where T : new()
{
    private static T instance;
    public static T Instance => instance ?? new T();
}
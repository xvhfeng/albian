namespace Albian.Kernel.AlbianCache
{
    public interface ICacheGroup
    {
        string Name { get; set;}
        int Size { get; set; }
        string Servers { get; set;}
    }
}
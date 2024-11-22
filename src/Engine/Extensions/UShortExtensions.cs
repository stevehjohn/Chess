namespace Engine.Extensions;

public static class UShortExtensions
{
    public static unsafe ushort Value(this ushort[] array, int index)
    {
        fixed (ushort* cell = array)
        {
            return cell[index];
        }
    }
}
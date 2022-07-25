namespace Nexus.Module
{
    public interface Identifiable<out T>
    {
        T GetIdentifier();
    }
}
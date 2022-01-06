namespace UserInterface
{
    public interface IParserFactory
    {
        IMessageParser GetParserFunc();
    }
}
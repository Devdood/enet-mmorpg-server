using HeroesServer;
using System.IO;

public abstract class PacketBase
{
    protected MemoryStream stream;
    protected BinaryReader reader;
    protected BinaryWriter writer;

    public PacketBase()
    {
        Init();
    }

    public void Init()
    {
        stream = new MemoryStream();
        SetReader(new BinaryReader(stream), stream);
    }

    public void SetReader(BinaryReader reader, MemoryStream stream)
    {
        this.stream = stream;
        this.reader = reader;

        writer = new BinaryWriter(stream);
        writer.Flush();
    }

    public abstract void Read(Client client);
    public abstract void Write();

    public byte[] GetWriteData()
    {
        return stream.ToArray();
    }
}

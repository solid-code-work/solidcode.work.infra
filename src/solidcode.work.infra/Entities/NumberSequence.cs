namespace Solidcode.Work.Infra.Entities;

public sealed class NumberSequence
{
    public Guid Id { get; private set; }

    public string DocumentType { get; private set; } = string.Empty;

    public int CurrentValue { get; private set; }

    public byte[] RowVersion { get; private set; } = [];

    private NumberSequence() { } // EF Core

    public NumberSequence(string documentType)
    {
        Id = Guid.NewGuid();
        DocumentType = documentType;
        CurrentValue = 0;
    }

    public void Increment()
    {
        CurrentValue++;
    }
}
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Logic.Domain.Level5Management.Contract.Enums.Archive;

namespace Logic.Domain.Level5Management.Contract.Archive
{
    public interface IArchiveTypeReader
    {
        ArchiveType Read(Stream input);
        ArchiveType Peek(Stream input);

        bool TryPeek(Stream input, [NotNullWhen(true)] out ArchiveType? type);
    }
}

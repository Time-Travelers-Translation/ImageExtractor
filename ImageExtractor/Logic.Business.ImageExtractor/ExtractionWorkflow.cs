using System.Diagnostics.CodeAnalysis;
using Logic.Business.ImageExtractor.InternalContract;
using Logic.Domain.Level5Management.Contract.Archive;
using Logic.Domain.Level5Management.Contract.DataClasses.Archive;
using Logic.Domain.Level5Management.Contract.DataClasses.Image;
using Logic.Domain.Level5Management.Contract.DataClasses.Script;
using Logic.Domain.Level5Management.Contract.Enums.Archive;
using Logic.Domain.Level5Management.Contract.Enums.Script;
using Logic.Domain.Level5Management.Contract.Image;
using Logic.Domain.Level5Management.Contract.Script;
using SixLabors.ImageSharp;

namespace Logic.Business.ImageExtractor
{
    internal class ExtractionWorkflow : IExtractionWorkflow
    {
        private const string CtrRepositoryBasePath_ = "G:/tt1_master_jpn/repositories/ctr/";
        private const string PspRepositoryBasePath_ = "G:/tt1_master_jpn/repositories/psp/";
        private const string DataBasePath_ = "G:/tt1_data_jpn/data/master/";

        private readonly ImageExtractorConfiguration _config;
        private readonly IStoryboardReader _storyboardReader;
        private readonly IArchiveTypeReader _archiveTypeReader;
        private readonly IArchiveReaderFactory _archiveReaderFactory;
        private readonly IImageParser _imageParser;

        public ExtractionWorkflow(ImageExtractorConfiguration config, IStoryboardReader storyboardReader,
            IArchiveTypeReader archiveTypeReader, IArchiveReaderFactory archiveReaderFactory, IImageParser imageParser)
        {
            _config = config;
            _storyboardReader = storyboardReader;
            _archiveTypeReader = archiveTypeReader;
            _archiveReaderFactory = archiveReaderFactory;
            _imageParser = imageParser;
        }

        public void Run()
        {
            string[] assetPaths = GetAssetPaths();
            ExtractAssets(assetPaths);
        }

        private string[] GetAssetPaths()
        {
            var paths = new HashSet<string>();

            var index = 1;

            string[] storyboardPaths = Directory.GetFiles(_config.GameFolder!, "*.stb", SearchOption.AllDirectories);
            foreach (string storyboardPath in storyboardPaths)
            {
                Console.Write($"\rProcess storyboard {index++}/{storyboardPaths.Length}...");

                if (!IsChapterStoryboard(storyboardPath, _config.Chapter))
                    continue;

                using Stream fileStream = File.OpenRead(storyboardPath);
                Storyboard storyboard = _storyboardReader.Read(fileStream);

                foreach (StoryboardInstruction instruction in storyboard.MainInstructions)
                {
                    if (instruction.Operation.OpCode != 0x15)
                        continue;

                    StoryboardValue[] parameters = instruction.Arguments[..^1];

                    foreach (StoryboardValue parameter in parameters)
                    {
                        if (parameter.Type != StoryboardValueType.String)
                            continue;

                        var stringValue = (string)parameter.Value!;
                        if (!TryNormalizePath(stringValue, out string? normalizedPath))
                            continue;

                        paths.Add(normalizedPath);
                    }
                }
            }

            Console.WriteLine(" Done");

            return paths.ToArray();
        }

        private void ExtractAssets(string[] assetPaths)
        {
            var index = 1;

            foreach (string assetPath in assetPaths)
            {
                Console.Write($"\rProcess asset {index++}/{assetPaths.Length}...");

                if (!TryGetFullAssetPath(assetPath, out string? fullAssetPath))
                    continue;

                using Stream archiveStream = File.OpenRead(fullAssetPath);

                if (!_archiveTypeReader.TryPeek(archiveStream, out ArchiveType? archiveType))
                    continue;

                IArchiveReader archiveReader = _archiveReaderFactory.Create(archiveType.Value);
                ArchiveData archiveData = archiveReader.Read(archiveStream);

                foreach (NamedArchiveEntry file in archiveData.Files)
                {
                    if (Path.GetExtension(file.Name) != ".xi")
                        continue;

                    ImageData imageData = _imageParser.Parse(file.Content);

                    string imageName = Path.GetFileNameWithoutExtension(file.Name);
                    string outputPath = GetFullOutputPath(assetPath, imageName);

                    imageData.Image.GetImage().SaveAsPng(outputPath);
                }
            }

            Console.WriteLine(" Done");
        }

        private string GetFullOutputPath(string assetPath, string imageName)
        {
            string outputPath = Path.Combine(_config.OutputFolder!, $"{_config.Chapter}");

            string? assetDirectory = Path.GetDirectoryName(assetPath);
            if (!string.IsNullOrEmpty(assetDirectory))
                outputPath = Path.Combine(outputPath, assetDirectory);

            Directory.CreateDirectory(outputPath);

            string assetName = Path.GetFileNameWithoutExtension(assetPath);
            outputPath = Path.Combine(outputPath, assetName);

            outputPath += $".{imageName}.png";

            return outputPath;
        }

        private static bool IsChapterStoryboard(string filePath, int chapter)
        {
            string fileName = Path.GetFileName(filePath);
            if (fileName.Length < 3)
                return false;

            string fileChapter = fileName[1..3];
            if (!int.TryParse(fileChapter, out int parsedChapter))
                return false;

            return parsedChapter == chapter;
        }

        private bool TryGetFullAssetPath(string assetPath, [NotNullWhen(true)] out string? fullAssetPath)
        {
            fullAssetPath = Path.Combine(_config.GameFolder!, "ctr", assetPath);
            if (File.Exists(fullAssetPath))
                return true;

            fullAssetPath = Path.Combine(_config.GameFolder!, "psp", assetPath);
            if (File.Exists(fullAssetPath))
                return true;

            fullAssetPath = null;
            return false;
        }

        private bool TryNormalizePath(string value, [NotNullWhen(true)] out string? normalizedPath)
        {
            if (value.StartsWith(DataBasePath_))
            {
                normalizedPath = value[DataBasePath_.Length..];
                return true;
            }

            if (value.StartsWith(CtrRepositoryBasePath_))
            {
                normalizedPath = value[CtrRepositoryBasePath_.Length..];
                return true;
            }

            if (value.StartsWith(PspRepositoryBasePath_))
            {
                normalizedPath = value[PspRepositoryBasePath_.Length..];
                return true;
            }

            normalizedPath = null;
            return false;
        }
    }
}

using Logic.Business.ImageExtractor.InternalContract;

namespace Logic.Business.ImageExtractor
{
    internal class ConfigurationValidator : IConfigurationValidator
    {
        public void Validate(ImageExtractorConfiguration config)
        {
            if (config.ShowHelp)
                return;

            ValidateChapter(config);
            ValidateInputPath(config);
            ValidateOutputPath(config);
        }

        private void ValidateChapter(ImageExtractorConfiguration config)
        {
            if (config.Chapter is < 1 or > 7)
                throw new InvalidOperationException("Chapter can only be 1-7.");
        }

        private void ValidateInputPath(ImageExtractorConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.GameFolder))
                throw new InvalidOperationException("No input folder path given. Specify one by using the -i argument.");

            if (!Directory.Exists(config.GameFolder))
                throw new InvalidOperationException($"Folder path '{config.GameFolder}' does not exist.");
        }

        private void ValidateOutputPath(ImageExtractorConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.OutputFolder))
                throw new InvalidOperationException("No output folder path given. Specify one by using the -o argument.");
        }
    }
}

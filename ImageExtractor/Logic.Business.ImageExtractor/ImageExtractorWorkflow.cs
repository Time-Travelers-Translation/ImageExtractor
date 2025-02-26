using Logic.Business.ImageExtractor.Contract;
using Logic.Business.ImageExtractor.InternalContract;

namespace Logic.Business.ImageExtractor
{
    internal class ImageExtractorWorkflow : IImageExtractorWorkflow
    {
        private readonly ImageExtractorConfiguration _config;
        private readonly IConfigurationValidator _configValidator;
        private readonly IExtractionWorkflow _extractionWorkflow;

        public ImageExtractorWorkflow(ImageExtractorConfiguration config, IConfigurationValidator configValidator,
            IExtractionWorkflow extractionWorkflow)
        {
            _config = config;
            _configValidator = configValidator;
            _extractionWorkflow = extractionWorkflow;
        }

        public int Execute()
        {
            if (_config.ShowHelp || Environment.GetCommandLineArgs().Length <= 1)
            {
                PrintHelp();
                return 0;
            }

            if (!IsValidConfig())
            {
                PrintHelp();
                return 0;
            }

            _extractionWorkflow.Run();

            return 0;
        }

        private bool IsValidConfig()
        {
            try
            {
                _configValidator.Validate(_config);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Input parameters are incorrect: {GetInnermostException(e).Message}");
                Console.WriteLine();

                return false;
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("Following commands exist:");
            Console.WriteLine("  -h, --help\t\tShows this help message.");
            Console.WriteLine("  -c, --chapter\t\tThe chapter to extract images from.");
            Console.WriteLine("  -i, --input\t\tThe folder path to the fully extracted tt1.cpk.");
            Console.WriteLine("  -o, --output\t\tThe folder path to output the images to.");
        }

        private Exception GetInnermostException(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            return e;
        }
    }
}

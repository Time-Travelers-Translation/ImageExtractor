using CrossCutting.Core.Contract.Configuration.DataClasses;

namespace Logic.Business.ImageExtractor
{
    public class ImageExtractorConfiguration
    {
        [ConfigMap("CommandLine", new[] { "h", "help" })]
        public virtual bool ShowHelp { get; set; } = false;

        [ConfigMap("CommandLine", new[] { "c", "chapter" })]
        public virtual int Chapter { get; set; }

        [ConfigMap("CommandLine", new[] { "i", "input" })]
        public virtual string? GameFolder { get; set; }

        [ConfigMap("CommandLine", new[] { "o", "output" })]
        public virtual string? OutputFolder { get; set; }
    }
}
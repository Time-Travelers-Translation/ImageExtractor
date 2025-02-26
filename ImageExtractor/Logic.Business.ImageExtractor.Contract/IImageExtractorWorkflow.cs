using CrossCutting.Core.Contract.Aspects;
using Logic.Business.ImageExtractor.Contract.Exceptions;

namespace Logic.Business.ImageExtractor.Contract
{
    [MapException(typeof(ImageExtractorManagementException))]
    public interface IImageExtractorWorkflow
    {
        int Execute();
    }
}

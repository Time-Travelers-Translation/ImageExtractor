using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Business.ImageExtractor.InternalContract
{
    public interface IConfigurationValidator
    {
        void Validate(ImageExtractorConfiguration config);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Business.ImageExtractor.Contract.Exceptions
{
    [Serializable]
    public class ImageExtractorManagementException : Exception
    {
        public ImageExtractorManagementException()
        {
        }

        public ImageExtractorManagementException(string message) : base(message)
        {
        }

        public ImageExtractorManagementException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ImageExtractorManagementException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}

using Core.Utilities.Constants;
using System;

namespace DTOs.Image
{
    public class GetImageHandlerInput : IInput
    {
        public ImageType Type { get; set; }
        public Guid Id { get; set; }

        public GetImageHandlerInput(ImageType type, Guid id)
        {
            this.Type = type;
            this.Id = id;
        }
    }
}

using Core.Utilities.Results;
using DTOs.Image;

namespace Business.Abstract.Image
{
    public interface IGetImage
    {
        IDataResult<GetImageHandlerOutput> Handle(GetImageHandlerInput input);
    }
}
using Business.Abstract.Image;
using Core.Utilities.Results;
using DTOs.Image;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;

namespace Business.Concrete.Image
{
    public class GetImageHandler : IGetImage
    {
        IOrganizationService _service;
        public GetImageHandler(IOrganizationService service)
        {
            this._service = service;
        }

        public IDataResult<GetImageHandlerOutput> Handle(GetImageHandlerInput input)
        {
            FileData fileData = null;
            if (input.Type == Core.Utilities.Constants.ImageType.Survey)
                fileData = GetFileData("hz_survey", "hz_image", input.Id);
            else if (input.Type == Core.Utilities.Constants.ImageType.AnswerOption)
                fileData = GetFileData("hz_surveyansweroption", "hz_image", input.Id);
            else if (input.Type == Core.Utilities.Constants.ImageType.Question)
                fileData = GetFileData("hz_surveyquestion", "hz_image", input.Id);
            
            if (fileData == null)
                return new ErrorDataResult<GetImageHandlerOutput>(null, "File not found");

            return new SuccessDataResult<GetImageHandlerOutput>(new GetImageHandlerOutput { File = fileData });
        }

        private FileData GetFileData(string entityLogicalName, string attributeLogicalName, Guid recordId)
        {
            var initRequest = new InitializeFileBlocksDownloadRequest() { FileAttributeName = attributeLogicalName, Target = new EntityReference(entityLogicalName, recordId) };
            var initResponse = (InitializeFileBlocksDownloadResponse)_service.Execute(initRequest);

            var increment = 4194304;
            var from = 0;
            var fileSize = initResponse.FileSizeInBytes;
            byte[] downloaded = new byte[fileSize];
            var fileContinuationToken = initResponse.FileContinuationToken;

            while (from < fileSize)
            {
                var blockRequest = new DownloadBlockRequest() { Offset = from, BlockLength = increment, FileContinuationToken = fileContinuationToken };
                var blockResponse = (DownloadBlockResponse)_service.Execute(blockRequest);
                blockResponse.Data.CopyTo(downloaded, from);
                from += increment;
            }

            return new FileData
            {
                File = downloaded,
                Mimetype = MimeMapping.MimeUtility.GetMimeMapping(initResponse.FileName),
                Filename = initResponse.FileName
            };
        }
    }
}

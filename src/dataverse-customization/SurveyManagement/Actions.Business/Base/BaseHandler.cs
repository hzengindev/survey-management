using Microsoft.Xrm.Sdk;
using System;

namespace Actions.Business.Base
{
    public class BaseHandler : IDisposable
    {
        protected IOrganizationService Service { get; set; }
        protected Guid? UserId { get; set; }

        public BaseHandler(IOrganizationService service)
        {
            Service = service;
        }

        /// <summary>
        /// For localization sources usage
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        public BaseHandler(IOrganizationService service, Guid userId)
        {
            Service = service;
            UserId = userId;
        }

        ~BaseHandler()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserId = null;
                Service = null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MyMicroService.Shared.Services
{
    public interface ISharerdIdentityService
    {
        string GetUserId {  get; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.ImageOptimizer
{
    public interface IMovInfoImageOptimizer
    {
        Task<string> OptimizeImage(IFormFile inputImage, int endHeight, int endWidth);

        void DeleteOldImage(string imageName);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace monsterCompendium.Services
{
    public interface IPhotoService
    {
        Task<string?> CapturePhotoAsync(string monsterIndex);
    }
}

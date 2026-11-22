using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace monsterCompendium.Services
{
    public class PhotoService : IPhotoService
    {
        public async Task<string?> CapturePhotoAsync(string monsterIndex)
        {
            var permissionStatus =
                await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (permissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            if (!MediaPicker.Default.IsCaptureSupported)
            {
                return null;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo is null)
            {
                return null;
            }

            return await SavePhotoAsync(photo, monsterIndex);
        }

        private static async Task<string> SavePhotoAsync(FileResult photo, string monsterIndex)
        {
            string photosDirectory = Path.Combine(FileSystem.AppDataDirectory, "monster-photos");

            Directory.CreateDirectory(photosDirectory);

            string extension = Path.GetExtension(photo.FileName);

            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".jpg";
            }

            string fileName = $"{monsterIndex}{extension}";

            string destinationPath =Path.Combine(photosDirectory, fileName);

            await using Stream sourceStream = await photo.OpenReadAsync();

            await using FileStream destinationStream = File.Create(destinationPath);

            await sourceStream.CopyToAsync(destinationStream);

            return destinationPath;
        }
    }
}

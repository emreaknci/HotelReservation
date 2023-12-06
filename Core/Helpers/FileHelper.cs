using Core.Utils.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class FileHelper
    {
        private static readonly string _currentDirectory = Environment.CurrentDirectory + "\\wwwroot\\";
        private static readonly string _folderName = "Images\\";
        public static Result<string> Upload(IFormFile file)
        {
            var fileExists = CheckFileExists(file);
            if (!fileExists.Success)
            {
                return Result<string>.FailureResult(fileExists.Message);
            }

            var type = Path.GetExtension(file.FileName);
            var typeValid = CheckFileTypeValid(type);
            var randomName = Guid.NewGuid().ToString();

            if (!typeValid.Success)
            {
                return Result<string>.FailureResult(typeValid.Message);
            }

            CheckDirectoryExists(_currentDirectory + _folderName);
            CreateImageFile(_currentDirectory + _folderName + randomName + type, file);
            return Result<string>.SuccessResult(randomName + type, "Dosya yüklendi!");
        }
        public static Result<List<string>> UploadRange(IFormCollection files)
        {
            List<string> filePathsList = new();
            foreach (var file in files.Files)
            {
                var result = Upload(file);
                if (result.Success) filePathsList.Add(result.Data);
            }
            return filePathsList.Count > 0 ? Result<List<string>>.SuccessResult(filePathsList, "Dosyalar yüklendi!") : Result<List<string>>.FailureResult("");
        }

        public static Result<string> Update(IFormFile newFile, string oldImagePath)
        {
            var fileExists = CheckFileExists(newFile);
            if (fileExists.Message != null)
            {
                return Result<string>.FailureResult(fileExists.Message);
            }

            var type = Path.GetExtension(newFile.FileName);
            var typeValid = CheckFileTypeValid(type);
            var randomName = Guid.NewGuid().ToString();

            if (typeValid.Message != null)
            {
                return Result<string>.FailureResult(typeValid.Message);
            }

            var x = (_currentDirectory + oldImagePath).Replace("/", "\\");
            DeleteOldImageFile((_currentDirectory + _folderName + oldImagePath).Replace("/", "\\"));
            CheckDirectoryExists(_currentDirectory + _folderName);
            CreateImageFile(_currentDirectory + _folderName + randomName + type, newFile);
            return Result<string>.SuccessResult(randomName + type, (_folderName + randomName + type).Replace("\\", "/"));
        }

        public static Result<bool> Delete(string path)
        {
            DeleteOldImageFile((_currentDirectory + path).Replace("/", "\\"));
            return Result<bool>.SuccessResult(true, "Dosya silindi!");
        }

        private static Result<bool> CheckFileExists(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                return Result<bool>.SuccessResult(true);
            }
            return Result<bool>.FailureResult("Dosya bulunumadı.");
        }

        private static Result<bool> CheckFileTypeValid(string type)
        {
            if (type != ".jpeg" && type != ".png" && type != ".jpg")
            {
                return Result<bool>.FailureResult("Geçersiz dosya uzantısı.");
            }
            return Result<bool>.SuccessResult(true);
        }

        private static void CheckDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void CreateImageFile(string directory, IFormFile file)
        {
            using FileStream fs = File.Create(directory);
            file.CopyTo(fs);
            fs.Flush();
        }

        private static void DeleteOldImageFile(string directory)
        {
            if (File.Exists(directory.Replace("/", "\\")))
            {
                File.Delete(directory.Replace("/", "\\"));
            }
        }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace YSManagmentSystem.web.DTO
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var context = (CmsShoppingCartContext)validationContext.GetService(typeof(CmShoppingCart));

            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg" };
                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return "Allowed extensions are jpg,png and jpeg.";
        }
    }
}

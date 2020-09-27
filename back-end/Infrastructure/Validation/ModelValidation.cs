using Abstractions.Exceptions;
using Abstractions.ISecurity;
using Abstractions.Model;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Validation
{
    /* Yes, the methods are almost the same and the errors are almost the same.
     * Yes, copy-pasted code all over the place.
     * 
     * But from the business stand point, the entities are different; the use-cases are different.
     * 
     * And if (or when) the validation will change, 
     * it will be more simple and more safe to change behavior for single use-case for a single entity,
     * rather than rewriting big and weird generic method
     */

    internal static class ModelValidation
    {
        //Introduction
        public static void CheckBeforeUpdate(DataModel.Introduction dbItem, Introduction introduction)
        {
            if (dbItem == null)
            {
                throw new InconsistencyException(Resources.TextMessages.IntroductionIsMissing);
            }


            if (dbItem.Version != introduction.Version)
            {
                throw new InconsistencyException
                (
                    string.Format(Resources.TextMessages.ItemWasAlreadyChanged, introduction.GetType().Name)
                );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = introduction.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                    continue;

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }

        }
        //



        //Account
        public static void CheckBeforeDelete(DataModel.Account dbItem, Account account)
        {
            if (account.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, account.GetType().Name)
                    );
            }

            if(dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if(dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }
        }
        
        public static void CheckBeforeUpdate(DataModel.Account dbItem, Account account, DataContext context)
        {
            if (account.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, account.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, account.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Role))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Role")
                    );
            }


            if (dbItem.Version != account.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, account.GetType().Name)
                    );
            }
        
            if(dbItem.Login != account.Login && context.Accounts.Any(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }

        public static void CheckBeforeCreate(Account account, DataContext context)
        {
            if (account.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, account.GetType().Name)
                    );
            }
           
            if (string.IsNullOrEmpty(account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Login")
                    );
            }

            if (string.IsNullOrEmpty(account.Password))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Password")
                    );
            }

            if (context.Accounts.Any(x => x.Login == account.Login))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Login")
                    );
            }

        }
        //


        // Category
        public static void CheckBeforeDelete(DataModel.Category dbItem, Category category, DataContext context)
        {
            if (category.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, category.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.IsEverything)
            {
                throw new InconsistencyException(Resources.TextMessages.CantDeleteSystemCategory);
            }

            var catWithProjects = context.CategoriesWithTotalProjects.FirstOrDefault(x => x.Id == category.Id);
            if (catWithProjects == null)
            {
                throw new InconsistencyException
                (
                    string.Format(Resources.TextMessages.CantDeleteNewItem, category.GetType().Name)
                );
            }


            if (catWithProjects.TotalProjects > 0)
            {
                throw new InconsistencyException
                    (
                    string.Format(Resources.TextMessages.CantDeleteNotEmptyCategory, catWithProjects.TotalProjects, catWithProjects.DisplayName)
                    );
            }
        }

        public static void CheckBeforeCreate(Category category, DataContext context)
        {
            if (category.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, category.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "DisplayName")
                    );
            }

            if (context.Categories.Any(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

        }

        public static void CheckBeforeUpdate(DataModel.Category dbItem, Category category, DataContext context)
        {
            if (category.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, category.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, category.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(category.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "DisplayName")
                    );
            }

            if (dbItem.Version != category.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, category.GetType().Name)
                    );
            }

            if (dbItem.Code != category.Code && context.Categories.Any(x => x.Code == category.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

        }
        //


        //Project
        public static void CheckBeforeDelete(DataModel.Project dbItem, Project project)
        {
            if (project.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantDeleteNewItem, project.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

        }

        public static void CheckBeforeCreate(Project project, DataContext context)
        {
            if (project.Id != null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantCreateExistingItem, project.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "DisplayName")
                    );
            }

            if (project.Category == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Category")
                    );
            }

            if (!context.Categories.Any(x=>x.Id == project.Category.Id))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.TheCategoryDoesNotExist, project.Category.Code)
                    );
            }


            if (context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if(string.IsNullOrEmpty(item.DisplayName))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name of the External URL")
                    );

                if (string.IsNullOrEmpty(item.Url))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "URL of the External URL")
                    );
            }

            foreach (var item in project.GalleryImages ?? new List<GalleryImage>())
            {
                if (string.IsNullOrEmpty(item.ImageUrl))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Image of the Gallery Image")
                    );
            }

        }

        public static void CheckBeforeUpdate(DataModel.Project dbItem, Project project, DataContext context)
        {
            if (project.Id == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.CantUpdateNewItem, project.GetType().Name)
                    );
            }

            if (dbItem == null)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.WasAlreadyDeleted, project.GetType().Name)
                    );
            }

            if (string.IsNullOrEmpty(project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Code")
                    );
            }

            if (string.IsNullOrEmpty(project.DisplayName))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "DisplayName")
                    );
            }

            if (dbItem.Version != project.Version)
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, project.GetType().Name)
                    );
            }

            if (dbItem.Code != project.Code && context.Projects.Any(x => x.Code == project.Code))
            {
                throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.PropertyDuplicate, "Code")
                    );
            }

            foreach (var item in dbItem.ExternalUrls)
            {
                var updated = project.ExternalUrls.FirstOrDefault(x => x.Id == item.ExternalUrlId);

                if (updated == null)
                    continue;

                if (item.ExternalUrl.Version != updated.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, updated.GetType().Name)
                    );
                }
            }


            foreach (var item in dbItem.GalleryImages)
            {
                var newUrl = project.GalleryImages.FirstOrDefault(x => x.Id == item.Id);
                if (newUrl == null)
                    continue;

                if (dbItem.Version != project.Version)
                {
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ItemWasAlreadyChanged, item.GetType().Name)
                    );
                }
            }

            foreach (var item in project.ExternalUrls ?? new List<ExternalUrl>())
            {
                if (string.IsNullOrEmpty(item.DisplayName))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Display name of the External URL")
                    );

                if (string.IsNullOrEmpty(item.Url))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "URL of the External URL")
                    );
            }

            foreach (var item in project.GalleryImages ?? new List<GalleryImage>())
            {
                if (string.IsNullOrEmpty(item.ImageUrl))
                    throw new InconsistencyException
                    (
                        string.Format(Resources.TextMessages.ThePropertyCantBeEmpty, "Image of the Gallery Image")
                    );
            }
        }

        //


        public static void CheckAccount(DataModel.Account dbItem, string login, string password, IHashManager hashManager)
        {
            if(string.IsNullOrEmpty(login))
            {
                throw new SecurityException(Resources.TextMessages.AccountEmptyLogin);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new SecurityException(Resources.TextMessages.AccountEmptyPassword);
            }

            if(dbItem == null)
            {
                throw new SecurityException(Resources.TextMessages.AccountSomethingWrong);
            }

            var hashedPassword = hashManager.Hash(password, dbItem.Salt);
            if (hashedPassword.HexHash != dbItem.Password)
            {
                throw new SecurityException(Resources.TextMessages.AccountSomethingWrong);
            }

        }
    }

}

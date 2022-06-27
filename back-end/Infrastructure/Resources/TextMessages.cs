namespace Infrastructure.Resources
{
    /// <summary> User-friendly messages</summary>
    static class TextMessages
    {
        /// <summary> The {0} was already changed. Refresh editor. </summary>
        internal const string ItemWasAlreadyChanged = "The {0} was already changed. Refresh editor.";

        /// <summary> Database is corrupted: Introduction is missing. </summary>
        internal const string IntroductionIsMissing = "Database is corrupted: Introduction is missing.";

        /// <summary> Can't delete new {0} </summary>
        internal const string CantDeleteNewItem = "Can't delete new {0}";

        /// <summary> Can't delete system category </summary>
        internal const string CantDeleteSystemCategory = "Can't delete system category";

        /// <summary> {0} must be only one </summary>
        internal const string MustBeOnlyOne = "{0} must be only one";

        /// <summary> Can't delete category, there are {0} projects within {1} category </summary>
        internal const string CantDeleteNotEmptyCategory = "Can't delete category, there are some projects within {0} category";

        /// <summary> Can't update new {0} </summary>
        internal const string CantUpdateNewItem = "Can't update new {0}";

        /// <summary> Can't create existing {0} </summary>
        internal const string CantCreateExistingItem = "Can't create existing {0}";

        /// <summary> Can't delete new {0} </summary>
        internal const string WasAlreadyDeleted = "{0} was already deleted";

        /// <summary> The property {0} can't be empty </summary>
        internal const string ThePropertyCantBeEmpty = "The {0} can't be empty";

        /// <summary> The category {0} does not exist </summary>
        internal const string CategoryDoesNotExist = "Category {0} does not exist";

        /// <summary> Wrong paging query </summary>
        internal const string WrongPagingQuery = "Wrong paging query";

        /// <summary> {0} duplicate </summary>
        internal const string PropertyDuplicate = "{0} duplicate";

        /// <summary> Role {0} does not exist </summary>
        internal const string TheRoleDoesNotExist = "Role {0} does not exist";

        /// <summary> Login is empty </summary>
        internal const string AccountEmptyLogin = "Login is empty";

        /// <summary> Password is empty </summary>
        internal const string AccountEmptyPassword = "Password is empty";

        /// <summary> Wrong login or password </summary>
        internal const string AccountSomethingWrong = "Wrong login or password";

        /// <summary> File is empty </summary>
        internal const string FileIsEmpty = "File is empty";

        /// <summary> File is too big. Max file size if {0} </summary>
        internal const string FileIsTooBig = "File is too big. Max file size if {0} ";

        /// <summary> Project does not exist </summary>
        internal const string ProjectDoesNotExist = "Project does not exist";

        /// <summary> Suspicious behavior </summary>
        internal const string SuspiciousBehavior = "Suspicious behavior";
    }
}

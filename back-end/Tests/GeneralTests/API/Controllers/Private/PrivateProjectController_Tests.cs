using Abstractions.Model;
using Abstractions.Supervision;
using API.Controllers.Gateway;
using API.Controllers.Private;
using API.Controllers.Public;
using API.Model;
using GeneralTests.Utils;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.API.Controllers.Private
{
    public class PrivateProjectController_Tests
    {
        class DefaultProjects : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Id = 1,
                        Code = "placeholder_code",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = null,
                        ReleaseDate = null,
                        GalleryImages = new List<GalleryImage>(),
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        },
                        Category = new Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 0,
                    }
                };

            }
        }

        class InvalidProjects : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Id = null,
                        Code = "placeholder_code",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = null,
                        ReleaseDate = null,
                        GalleryImages = new List<GalleryImage>(),
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 0
                            }
                        },
                        Category = new Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 0,
                    }
                };

            }
        }



        private static AuthenticationController CreateAuthenticationController(DataContext context)
        {
            var confing = Storage.CreateConfiguration();
            var hashManager = new HashManager();
            var accountRep = new AccountRepository(context, confing, hashManager);
            var tokenManager = new TokenManager(confing);
            var sup = new Supervisor(tokenManager);

            return new AuthenticationController(confing, accountRep, sup, tokenManager);
        }

        private static PublicProjectController CreatePublicProjectController(DataContext context)
        {
            var categoryRep = new CategoryRepository(context);
            var projectRep = new ProjectRepository(context, categoryRep);
            var tokenManager = new TokenManager(Storage.CreateConfiguration());
            var sup = new Supervisor(tokenManager);

            return new PublicProjectController(projectRep, sup);
        }

        private static PublicCategoryController CreatePublicCategoryController(DataContext context)
        {
            var categoryRep = new CategoryRepository(context);
            var tokenManager = new TokenManager(Storage.CreateConfiguration());
            var sup = new Supervisor(tokenManager);

            return new PublicCategoryController(categoryRep, sup);
        }


        private static PrivateProjectController CreatePrivateProjectController(DataContext context)
        {
            var config = Storage.CreateConfiguration();
            var fileRep = new FileRepository(config);
            var categoryRep = new CategoryRepository(context);
            var projectRep = new ProjectRepository(context, categoryRep);
            var tokenManager = new TokenManager(config);
            var sup = new Supervisor(tokenManager);

            return new PrivateProjectController(config, fileRep, projectRep, sup);
        }

        private static PrivateAccountController CreatePrivateAccountController(DataContext context)
        {
            var confing = Storage.CreateConfiguration();
            var tokenManager = new TokenManager(confing);
            var hasManager = new HashManager();
            var sup = new Supervisor(tokenManager);
            var accRep = new AccountRepository(context, confing, hasManager);

            return new PrivateAccountController(accRep, sup);
        }

        // --
        [Theory]
        [ClassData(typeof(DefaultProjects))]
        internal async void DeleteProject_Valid(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);


                    var apiPrivate = CreatePrivateProjectController(context);
                    
                    var response =
                    (
                        await apiPrivate.Delete(identity.Data.Token, project) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckSucceed(response);
                    Assert.True(response.Data);

                    var apiProjectPublic = CreatePublicProjectController(context);
                    var responseGet =
                    (
                        await apiProjectPublic.GetProject(project.Code) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckFail(responseGet);

                    var apiCategoryPublic = CreatePublicCategoryController(context);
                    var responseGetCategory =
                    (
                        await apiCategoryPublic.GetCategory(project.Category.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategory);

                    Assert.Equal(0, responseGetCategory.Data.TotalProjects);

                    var responseGetCategoryEverything =
                    (
                        await apiCategoryPublic.GetEverythingCategory() as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategoryEverything);
                    Assert.Equal(0, responseGetCategoryEverything.Data.TotalProjects);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void DeleteProject_Invalid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void DeleteProject_Badtoken()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void DeleteProject_InsufficientRights()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var apiAccount = CreatePrivateAccountController(context);
                    var createDemo = await apiAccount.SaveAsync
                    (
                        identity.Data.Token, new Account { Login = "demo", Password = "demo", Role = RoleNames.Demo }
                    );
                    var demoIdentity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "demo", Password = "demo" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        // --

        internal async void CreateProject_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void CreateProject_Invalid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void CreateProject_Badtoken()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void CreateProject_InsufficientRights()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var apiAccount = CreatePrivateAccountController(context);
                    var createDemo = await apiAccount.SaveAsync
                    (
                        identity.Data.Token, new Account { Login = "demo", Password = "demo", Role = RoleNames.Demo }
                    );
                    var demoIdentity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "demo", Password = "demo" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        // --

        internal async void UpdateProject_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void UpdateProject_Invalid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void UpdateProject_Badtoken()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        internal async void UpdateProject_InsufficientRights()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiAuth = CreateAuthenticationController(context);
                    var identity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "sa", Password = "sa" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                    var apiAccount = CreatePrivateAccountController(context);
                    var createDemo = await apiAccount.SaveAsync
                    (
                        identity.Data.Token, new Account { Login = "demo", Password = "demo", Role = RoleNames.Demo }
                    );
                    var demoIdentity =
                    (
                        await apiAuth.LoginAsync(new Credentials { Login = "demo", Password = "demo" }) as JsonResult
                    ).Value as ExecutionResult<Identity>;
                    GenericChecks.CheckSucceed(identity);

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushDatabase();
                }
            }
        }

        // --
    }
}

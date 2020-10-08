﻿using Abstractions.Model;
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
                        GalleryImages = new List<GalleryImage>()
                        {
                            new GalleryImage
                            {
                                Id = 1,
                                ExtraUrl = null,
                                ImageUrl = "https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png",
                                Version = 0
                            }
                        },
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

        class InvalidDeleteProjects : IEnumerable<object[]>
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
                        GalleryImages = new List<GalleryImage>()
                        {
                            new GalleryImage
                            {
                                Id = 1,
                                ExtraUrl = null,
                                ImageUrl = "https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png",
                                Version = 0
                            }
                        },
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
                        GalleryImages = new List<GalleryImage>()
                        {
                            new GalleryImage
                            {
                                Id = 1,
                                ExtraUrl = null,
                                ImageUrl = "https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png",
                                Version = 0
                            }
                        },
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
                        Version = 10,
                    }
                };
                yield return new object[]
                {
                    new Project
                    {
                        Id = 12,
                        Code = "placeholder_code",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = null,
                        ReleaseDate = null,
                        GalleryImages = new List<GalleryImage>()
                        {
                            new GalleryImage
                            {
                                Id = 1,
                                ExtraUrl = null,
                                ImageUrl = "https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png",
                                Version = 0
                            }
                        },
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

        class NewProjects : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = null,
                        ReleaseDate = null,
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

                yield return new object[]
                {
                    new Project
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = "http://12",
                        ReleaseDate = null,
                        GalleryImages = new List<GalleryImage>()
                        {
                            new GalleryImage
                            {
                                Id = null,
                                ExtraUrl = null,
                                ImageUrl = "https://raw.githubusercontent.com/ChiefNoir/BusinessCard/master/front-end/BusinessSite/src/assets/images/placeholder-wide.png",
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

                yield return new object[]
                {
                    new Project
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort ="The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = null,
                        ReleaseDate = DateTime.Now,
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = null,
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

        [Theory]
        [ClassData(typeof(InvalidDeleteProjects))]
        internal async void DeleteProject_Invalid(Project project)
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
                    GenericChecks.CheckFail(response);
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

        [Theory]
        [ClassData(typeof(DefaultProjects))]
        internal async void DeleteProject_Badtoken(Project project)
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
                        await apiPrivate.Delete("bad-token", project) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckFail(response);
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

        [Theory]
        [ClassData(typeof(DefaultProjects))]
        internal async void DeleteProject_InsufficientRights(Project project)
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

                    var apiPrivate = CreatePrivateProjectController(context);
                    var response =
                    (
                        await apiPrivate.Delete(demoIdentity.Data.Token, project) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckFail(response);
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

        [Theory]
        [ClassData(typeof(NewProjects))]
        internal async void CreateProject_Valid(Project project)
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
                        await apiPrivate.Save(identity.Data.Token, project) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckSucceed(response);

                    Compare(project, response.Data);

                    var apiCategoryPublic = CreatePublicCategoryController(context);
                    var responseGetCategory =
                    (
                        await apiCategoryPublic.GetCategory(project.Category.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategory);

                    Assert.Equal(project.Category.TotalProjects + 1, responseGetCategory.Data.TotalProjects);

                    var responseGetCategoryEverything =
                    (
                        await apiCategoryPublic.GetEverythingCategory() as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategoryEverything);
                    Assert.Equal(project.Category.TotalProjects + 1, responseGetCategoryEverything.Data.TotalProjects);

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

        [Theory]
        [ClassData(typeof(NewProjects))]
        internal async void CreateProject_Badtoken(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var apiPrivate = CreatePrivateProjectController(context);
                    var response =
                    (
                        await apiPrivate.Save("bad-token", project) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckFail(response);
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

        [Theory]
        [ClassData(typeof(NewProjects))]
        internal async void CreateProject_InsufficientRights(Project project)
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


                    var apiPrivate = CreatePrivateProjectController(context);
                    var response =
                    (
                        await apiPrivate.Save(demoIdentity.Data.Token, project) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckFail(response);
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


        private static void Compare(Project expected, Project actual)
        {
            Assert.NotNull(actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.DescriptionShort, actual.DescriptionShort);
            Assert.Equal(expected.DisplayName, actual.DisplayName);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate?.Date, actual.ReleaseDate?.Date);

            Assert.Equal(expected.Version, actual.Version);


            Compare(expected.Category, actual.Category);

            Assert.Equal(expected.ExternalUrls?.Count() ?? 0, actual.ExternalUrls?.Count() ?? 0);

            foreach (var expectedUrl in expected.ExternalUrls ?? new List<ExternalUrl>())
            {
                var actualUrl = actual.ExternalUrls.FirstOrDefault(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.Equal(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.Equal(expectedUrl.Url, actualUrl.Url);
                Assert.Equal(expectedUrl.Version, actualUrl.Version);
            }

            Assert.Equal(expected.GalleryImages?.Count() ?? 0, actual.GalleryImages?.Count() ?? 0);

            foreach (var expectedImg in expected.GalleryImages ?? new List<GalleryImage>())
            {
                var actualUrl = actual.GalleryImages.FirstOrDefault(x => x.ImageUrl == expectedImg.ImageUrl);

                Assert.Equal(expectedImg.ImageUrl, actualUrl.ImageUrl);
                Assert.Equal(expectedImg.ExtraUrl, actualUrl.ExtraUrl);
                Assert.Equal(expectedImg.Version, actualUrl.Version);
            }
        }

        private static void Compare(Category expected, Category actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.DisplayName, actual.DisplayName);

            Assert.False(expected.IsEverything);
            Assert.Equal(expected.IsEverything, actual.IsEverything);
            Assert.Equal(expected.Version, actual.Version);
        }

    }
}

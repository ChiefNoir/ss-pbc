using Abstractions.Model;
using Abstractions.Model.Queries;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.API.Controllers.Public
{
    public class PublicProjectController_Tests
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

        class ValidProjectsPreview : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    " insert into project (id,code,display_name, category_id, description_short, description) "+
                        " values (2,'games-1','games-title-1', 2,'games-1: short description','games-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(3, 'games-2', 'games-title-2', 2, 'games-2: short description', 'games-2: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(4, 'comics-1', 'Comics-title-1', 3, 'Comics-1: short description', 'Comics-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(5, 'stories-1', 'Stories-title-1', 4, 'Stories-1: short description', 'Stories-1: long description'); "+
                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(6, 'stories-2', 'Stories-title-2', 4, 'Stories-2: short description', 'Stories-2: long description'); ",
                    
                    new Paging {Start = 0, Length = 1 },
                    new ProjectSearch { CategoryCode = "s"},

                    new []
                    {
                        new ProjectPreview
                        {
                            Code = "placeholder_code",
                            DisplayName = "Brand new project",
                            Description = "The smart and short description.",
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
                            }
                        }
                    }
                };

                yield return new object[]
                {
                    " insert into project (id,code,display_name, category_id, description_short, description) "+
                        " values (2,'games-1','games-title-1', 2,'games-1: short description','games-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(3, 'games-2', 'games-title-2', 2, 'games-2: short description', 'games-2: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(4, 'comics-1', 'Comics-title-1', 3, 'Comics-1: short description', 'Comics-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(5, 'stories-1', 'Stories-title-1', 4, 'Stories-1: short description', 'Stories-1: long description'); "+
                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(6, 'stories-2', 'Stories-title-2', 4, 'Stories-2: short description', 'Stories-2: long description'); ",

                    new Paging {Start = 0, Length = 2 },
                    new ProjectSearch { CategoryCode = "lit"},

                    new []
                    {
                        new ProjectPreview
                        {
                            Code = "stories-1",
                            DisplayName = "Stories-title-1",
                            Description = "Stories-1: short description",
                            PosterDescription = null,
                            PosterUrl = null,
                            ReleaseDate = null,
                            Category = new Category
                            {
                                Id = 4,
                                Code = "lit",
                                DisplayName = "Stories",
                                IsEverything = false,
                                TotalProjects = 2,
                                Version = 0
                            }
                        },
                        new ProjectPreview
                        {
                            Code = "stories-2",
                            DisplayName = "Stories-title-2",
                            Description = "Stories-2: short description",
                            PosterDescription = null,
                            PosterUrl = null,
                            ReleaseDate = null,
                            Category = new Category
                            {
                                Id = 4,
                                Code = "lit",
                                DisplayName = "Stories",
                                IsEverything = false,
                                TotalProjects = 2,
                                Version = 0
                            }
                        },
                    }
                };

                yield return new object[]
                {
                    " insert into project (id,code,display_name, category_id, description_short, description) "+
                        " values (2,'games-1','games-title-1', 2,'games-1: short description','games-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(3, 'games-2', 'games-title-2', 2, 'games-2: short description', 'games-2: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description) "+
                        " values(4, 'comics-1', 'Comics-title-1', 3, 'Comics-1: short description', 'Comics-1: long description'); "+

                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(5, 'stories-1', 'Stories-title-1', 4, 'Stories-1: short description', 'Stories-1: long description'); "+
                    " insert into project(id, code, display_name, category_id, description_short, description)"+
                        " values(6, 'stories-2', 'Stories-title-2', 4, 'Stories-2: short description', 'Stories-2: long description'); ",

                    new Paging {Start = 0, Length = 2 },
                    new ProjectSearch { CategoryCode = "bg"},

                    new ProjectPreview [] {}
                };
            }
        }

        class InvalidEmptyProjectsPreview : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Paging {Start = 20, Length = 1 },
                    new ProjectSearch { CategoryCode = "s"},
                };

                yield return new object[]
                {
                    new Paging {Start = 0, Length = 2 },
                    new ProjectSearch { CategoryCode = "lit"},
                };

                yield return new object[]
                {
                    new Paging {Start = 0, Length = 2 },
                    new ProjectSearch { CategoryCode = "bg"},
                };
            }
        }

        class InvalidProjectsPreview : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Paging {Start = 0, Length = 10 },
                    new ProjectSearch { CategoryCode = "cute"},
                };
                yield return new object[]
                {
                    new Paging {Start = 0, Length = -10 },
                    new ProjectSearch { CategoryCode = "bg"},
                };
                yield return new object[]
                {
                    new Paging {Start = -10, Length = 0 },
                    new ProjectSearch { CategoryCode = "bg"},
                };
            }
        }


        [Theory]
        [ClassData(typeof(DefaultProjects))]
        internal async Task GetProject_ValidAsync(Project expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);

                    var response =
                    (
                        await api.GetProjectAsync(expected.Code) as JsonResult
                    ).Value as ExecutionResult<Project>;

                    GenericChecks.CheckSucceed(response);
                    Compare(expected, response.Data);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("qwerty")]
        internal async Task GetProject_InValidAsync(string code)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);

                    var response =
                    (
                        await api.GetProjectAsync(code) as JsonResult
                    ).Value as ExecutionResult<Project>;

                    GenericChecks.CheckFail(response);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(ValidProjectsPreview))]
        internal async Task GetProjectsPreview_ValidAsync(string sql, Paging paging, ProjectSearch projectSearch, ProjectPreview[] expectedProjects)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql(sql);

                    var api = Storage.CreatePublicController(context);


                    var response = 
                    (
                        await api.GetProjectsPreviewAsync(paging, projectSearch) as JsonResult
                    ).Value as ExecutionResult<ProjectPreview[]>;

                    GenericChecks.CheckSucceed(response);
                    Assert.Equal(expectedProjects.Length, response.Data.Length);

                    foreach (var item in expectedProjects)
                    {
                        var actual = response.Data.SingleOrDefault(x => x.Code == item.Code);
                        Compare(item, actual);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidProjectsPreview))]
        internal async Task GetProjectsPreview_InValidAsync(Paging paging, ProjectSearch projectSearch)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);


                    var response =
                    (
                        await api.GetProjectsPreviewAsync(paging, projectSearch) as JsonResult
                    ).Value as ExecutionResult<ProjectPreview[]>;
                    GenericChecks.CheckFail(response);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidEmptyProjectsPreview))]
        internal async Task GetProjectsPreview_InValidEmptyAsync(Paging paging, ProjectSearch projectSearch)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePublicController(context);


                    var response =
                    (
                        await api.GetProjectsPreviewAsync(paging, projectSearch) as JsonResult
                    ).Value as ExecutionResult<ProjectPreview[]>;
                    GenericChecks.CheckEmpty(response);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    context.FlushData();
                }
            }
        }


        private void Compare(Project expected, Project actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.DescriptionShort, actual.DescriptionShort);
            Assert.Equal(expected.DisplayName, actual.DisplayName);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);
            Assert.Equal(expected.Version, actual.Version);


            Compare(expected.Category, actual.Category);
            Assert.Equal(expected.ExternalUrls.Count(), actual.ExternalUrls.Count());

            foreach (var expectedUrl in expected.ExternalUrls)
            {
                var actualUrl = actual.ExternalUrls.FirstOrDefault(x => x.DisplayName == expectedUrl.DisplayName);

                Assert.Equal(expectedUrl.DisplayName, actualUrl.DisplayName);
                Assert.Equal(expectedUrl.Url, actualUrl.Url);
                Assert.Equal(expectedUrl.Version, actualUrl.Version);
            }

            Assert.Equal(expected.GalleryImages.Count(), actual.GalleryImages.Count());

            foreach (var expectedImg in expected.GalleryImages)
            {
                var actualUrl = actual.GalleryImages.FirstOrDefault(x => x.ImageUrl == expectedImg.ImageUrl);

                Assert.Equal(expectedImg.ImageUrl, actualUrl.ImageUrl);
                Assert.Equal(expectedImg.ExtraUrl, actualUrl.ExtraUrl);
                Assert.Equal(expectedImg.Version, actualUrl.Version);
            }
        }

        private void Compare(ProjectPreview expected, ProjectPreview actual)
        {
            Assert.Equal(expected.Code, actual.Code);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.DisplayName, actual.DisplayName);

            Assert.Equal(expected.PosterDescription, actual.PosterDescription);
            Assert.Equal(expected.PosterUrl, actual.PosterUrl);
            Assert.Equal(expected.ReleaseDate, actual.ReleaseDate);
            
            Compare(expected.Category, actual.Category);
        }

        private void Compare(Category expected, Category actual)
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

using Abstractions.ISecurity;
using Abstractions.Model;
using Abstractions.Model.System;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        class InvalidDelete : IEnumerable<object[]>
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

        class ValidUpdate : IEnumerable<object[]>
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
                                Id = 2,
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
                    },
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
                                Version = 1
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = 1,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 1
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
                        Version = 1,
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
                        ExternalUrls = new List<ExternalUrl>(),
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
                    },
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
                                Version = 1
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>(),
                        Category = new Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 1,
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
                        GalleryImages = new List<GalleryImage>(),
                        ExternalUrls = new List<ExternalUrl>(),
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
                    },
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
                        ExternalUrls = new List<ExternalUrl>(),
                        Category = new Category
                        {
                            Id = 6,
                            Code = "s",
                            DisplayName = "Software",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 1,
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
                        ExternalUrls = new List<ExternalUrl>(),
                        Category = new Category
                        {
                            Id = 2,
                            Code = "vg",
                            DisplayName = "Games",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 0,
                    },
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
                                Version = 1
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>(),
                        Category = new Category
                        {
                            Id = 2,
                            Code = "vg",
                            DisplayName = "Games",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 1,
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
                                Id = null,
                                DisplayName = "cute",
                                Url = "https://github.com/",
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
                    },
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
                                Version = 1
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = "cute",
                                Url = "https://github.com/",
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
                        Version = 1,
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
                                Id = null,
                                ExtraUrl = null,
                                ImageUrl = "something",
                                Version = 0
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = 2,
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
                    },
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
                                Id = 2,
                                ExtraUrl = null,
                                ImageUrl = "something",
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
                                Version = 1
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
                        Version = 1,
                    }
                };

            }
        }

        class InvalidUpdate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Id = 1,
                        Code = null,
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
                        Code = null,
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
                        Category = null,
                        Version = 0,
                    }
                };

                yield return new object[]
                {
                    new Project
                    {
                        Id = 1,
                        Code = "placeholder_code",
                        DisplayName = null,
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
                            Id = 16,
                            Code = "ctu",
                            DisplayName = "Cute",
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
                                DisplayName = null,
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
                                Url = null,
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
                                ImageUrl = null,
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
                        Code = "nice",
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
                                Id = 2,
                                DisplayName = "GitHub",
                                Url = "https://github.com/ChiefNoir",
                                Version = 10
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
                        DescriptionShort = "The smart and short description.",
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
                                    Version = 10
                                }
                            },
                        ExternalUrls = new List<ExternalUrl>()
                            {
                                new ExternalUrl
                                {
                                    Id = 2,
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

        class InvalidNew : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Project
                    {
                        Id = null,
                        Code = null,
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
                        DisplayName = null,
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
                        PosterUrl = null,
                        ReleaseDate = null,
                        Category = null,
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
                        ReleaseDate = null,
                        Category = new Category
                        {
                            Id = 16,
                            Code = "cute",
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
                                ImageUrl = null,
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
                                Url = null,
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
                                Url = null,
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
                        Code = "placeholder_code",
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
                        PosterUrl = null,
                        ReleaseDate = DateTime.Now,
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = null,
                                DisplayName = null,
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

        class ValidUpdateWithoutExpected : IEnumerable<object[]>
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
                                Id = 2,
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
                    },
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
                        ExternalUrls = new List<ExternalUrl>(),
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
                    },
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
                        GalleryImages = new List<GalleryImage>(),
                        ExternalUrls = new List<ExternalUrl>(),
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
                    },
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
                        ExternalUrls = new List<ExternalUrl>(),
                        Category = new Category
                        {
                            Id = 2,
                            Code = "vg",
                            DisplayName = "Games",
                            IsEverything = false,
                            TotalProjects = 1,
                            Version = 0
                        },
                        Version = 0,
                    },
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
                                Id = null,
                                DisplayName = "cute",
                                Url = "https://github.com/",
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
                    },
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
                                Id = null,
                                ExtraUrl = null,
                                ImageUrl = "something",
                                Version = 0
                            }
                        },
                        ExternalUrls = new List<ExternalUrl>()
                        {
                            new ExternalUrl
                            {
                                Id = 2,
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
                    },
                };

            }
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
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);

                    var response =
                    (
                        await api.DeleteProjectAsync(project) as JsonResult
                    ).Value as ExecutionResult<bool>;
                    GenericChecks.CheckSucceed(response);
                    Assert.True(response.Data);

                    var apiPublic = Storage.CreatePublicController(context);
                    var responseGet =
                    (
                        await apiPublic.GetProjectAsync(project.Code) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckFail(responseGet);

                    var apiCategoryPublic = Storage.CreatePublicController(context);
                    var responseGetCategory =
                    (
                        await apiCategoryPublic.GetCategoryAsync(project.Category.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategory);

                    Assert.Equal(0, responseGetCategory.Data.TotalProjects);

                    var responseGetCategoryEverything =
                    (
                        await apiCategoryPublic.GetCategoryEverythingAsync() as JsonResult
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
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidDelete))]
        internal async void DeleteProject_Invalid(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);

                    var response =
                    (
                        await api.DeleteProjectAsync(project) as JsonResult
                    ).Value as ExecutionResult<bool>;
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
        [ClassData(typeof(NewProjects))]
        internal async void CreateProject_Valid(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);
                    var response =
                    (
                        await api.SaveProjectAsync(project) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckSucceed(response);

                    Compare(project, response.Data);

                    var apiPublic = Storage.CreatePublicController(context);
                    var responseGetCategory =
                    (
                        await apiPublic.GetCategoryAsync(project.Category.Id.Value) as JsonResult
                    ).Value as ExecutionResult<Category>;
                    GenericChecks.CheckSucceed(responseGetCategory);

                    Assert.Equal(project.Category.TotalProjects + 1, responseGetCategory.Data.TotalProjects);

                    var responseGetCategoryEverything =
                    (
                        await apiPublic.GetCategoryEverythingAsync() as JsonResult
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
                    context.FlushData();
                }
            }
        }

        [Theory]
        [ClassData(typeof(InvalidNew))]
        internal async void CreateProject_Invalid(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);

                    var response =
                    (
                        await api.SaveProjectAsync(project) as JsonResult
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
        [ClassData(typeof(ValidUpdate))]
        internal async void UpdateProject_Valid(Project project, Project expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);
                    var response =
                    (
                        await api.SaveProjectAsync(project) as JsonResult
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
        [ClassData(typeof(InvalidUpdate))]
        internal async void UpdateProject_Invalid(Project project)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    Storage.RunSql("insert into project (code, display_name, category_id) values ('nice','temp',6);");

                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, null);

                    var response =
                    (
                        await api.SaveProjectAsync(project) as JsonResult
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


        [Fact]
        internal async void CreateProjectWithFiles_Valid()
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var project = new Project
                    {
                        Id = null,
                        Code = "cute",
                        DisplayName = "Brand new project",
                        Description = "Not that smart and pretty long description.",
                        DescriptionShort = "The smart and short description.",
                        PosterDescription = null,
                        PosterUrl = @"Files/untitled.png",
                        ReleaseDate = null,
                        GalleryImages = new []
                        {
                            new GalleryImage()
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
                    };

                    var streamPoster = File.OpenRead(project.PosterUrl);
                    var streamGallery = File.OpenRead(project.PosterUrl);

                    var ffcollection = new FormFileCollection
                    {
                        new FormFile(streamPoster, 0, streamPoster.Length, "project[posterToUpload]", "untitled.png"),
                        new FormFile(streamGallery, 0, streamPoster.Length, "project[galleryImages][0][readyToUpload]", "untitled.png"),
                    };

                    var api = Storage.CreatePrivateController(context);
                    api.ControllerContext = await ControllerContextCreator.CreateValid(context, ffcollection);
                    
                    var response =
                    (
                        await api.SaveProjectAsync(project) as JsonResult
                    ).Value as ExecutionResult<Project>;
                    GenericChecks.CheckSucceed(response);

                    Assert.NotNull(response.Data.PosterUrl);

                    var config = Storage.CreateConfiguration();
                    var pathStart = config.GetSection("Kestrel:Endpoints:Https:Url").Get<string>()
                        + "/" + config.GetSection("Location:FileStorage").Get<string>();

                    Assert.StartsWith(pathStart, response.Data.PosterUrl);

                    var storagePath = config.GetSection("Location:FileStorage").Get<string>();
                    var fileExists = File.Exists(Path.Combine(storagePath, Path.GetFileName(response.Data.PosterUrl)));
                    Assert.True(fileExists);



                    Assert.NotNull(response.Data.GalleryImages.FirstOrDefault());
                    Assert.StartsWith(pathStart, response.Data.GalleryImages.FirstOrDefault()?.ImageUrl);


                    var galleryExists = File.Exists(Path.Combine(storagePath, Path.GetFileName(response.Data.GalleryImages.FirstOrDefault()?.ImageUrl)));
                    Assert.True(galleryExists);
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

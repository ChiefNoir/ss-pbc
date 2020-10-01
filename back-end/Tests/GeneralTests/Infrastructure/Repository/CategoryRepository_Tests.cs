using GeneralTests.Utils;
using Infrastructure.DataModel;
using Infrastructure.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.Infrastructure.Repository
{
    public class CategoryRepository_Tests
    {
        class GenerateValidTechnical : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        Version = 0,
                    }
                };

            }
        }

        class GenerateValidDefaults : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new object[]
                    {
                    new Abstractions.Model.Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        Version = 0,
                    },
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category
                    {
                        Id = 3,
                        Code = "ma",
                        DisplayName = "Comics",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category
                    {
                        Id = 4,
                        Code = "lit",
                        DisplayName = "Stories",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category
                    {
                        Id = 5,
                        Code = "bg",
                        DisplayName = "Tabletop",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category
                    {
                        Id = 6,
                        Code = "s",
                        DisplayName = "Software",
                        IsEverything = false,
                        Version = 0,
                    },
                    }
                };

            }
        }

        class GenerateValidSave : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category //save
                    {
                        Id = 1,
                        Code = "allall",
                        DisplayName = "Everything-Everything",
                        IsEverything = true,
                        Version = 0,
                    },
                    new Abstractions.Model.Category //expected
                    {
                        Id = 1,
                        Code = "allall",
                        DisplayName = "Everything-Everything",
                        IsEverything = true,
                        Version = 1,
                    },
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category //save
                    {
                        Id = 4,
                        Code = "tttt",
                        DisplayName = "Everything",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category //expected
                    {
                        Id = 4,
                        Code = "tttt",
                        DisplayName = "Everything",
                        IsEverything = false,
                        Version = 1,
                    },
                };

                yield return new object[]
                {
                    new Abstractions.Model.Category //save
                    {
                        Id = null,
                        Code = "bigbrandnew",
                        DisplayName = "bigbrandnew",
                        IsEverything = false,
                        Version = 0,
                    },
                    new Abstractions.Model.Category //expected
                    {
                        Id = 7,
                        Code = "bigbrandnew",
                        DisplayName = "bigbrandnew",
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        class GenerateValidDelete : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 3,
                        Code = "ma",
                        DisplayName = "Comics",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 5,
                        Code = "bg",
                        DisplayName = "Tabletop",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 6,
                        Code = "s",
                        DisplayName = "Software",
                        IsEverything = false,
                        Version = 0,
                    },
                };
            }
        }

        class GenerateInValidUpdate: IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = null,
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 129,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = null,
                        DisplayName = "Games-Games",
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games1",
                        IsEverything = true,
                        Version = 10,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "s",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    }
                };
            }
        }

        class GenerateInValidCreate : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = false,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = null,
                        DisplayName = "Games",
                        IsEverything = false,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "",
                        DisplayName = "Games",
                        IsEverything = false,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "something",
                        DisplayName = "Games",
                        IsEverything = true,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "Ms",
                        DisplayName = "Games",
                        IsEverything = false,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "++",
                        DisplayName = "Games",
                        IsEverything = false,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "loncode",
                        DisplayName = null,
                        IsEverything = false,
                    }
                };
            }
        }

        class GenerateInValidDelete : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 1,
                        Code = "all",
                        DisplayName = "Everything",
                        IsEverything = true,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = null,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 0,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 2,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 10,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 122,
                        Code = "vg",
                        DisplayName = "Games",
                        IsEverything = false,
                        Version = 10,
                    }
                };
                yield return new object[]
                {
                    new Abstractions.Model.Category
                    {
                        Id = 4,
                        Code = "lit",
                        DisplayName = "Stories",
                        IsEverything = false,
                        Version = 0,
                    }
                };
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidTechnical))]
        internal async void GetTechnicalAsync_Valid(Abstractions.Model.Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);
                var result = await rep.GetTechnicalAsync();

                Compare(result, expected);

                context.FlushDatabase();
            }
        }

        [Theory]
        [ClassData(typeof(GenerateValidDefaults))]
        internal async void GetAsync_Valid(Abstractions.Model.Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);
                var result = await rep.GetAsync();

                foreach (var item in result)
                {
                    var expected = expectedCategory.FirstOrDefault(x => x.Code == item.Code);

                    Assert.NotNull(expected);

                    Compare(item, expected);
                }

                context.FlushDatabase();
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidDefaults))]
        internal async void GetAsyncByCode_Valid(Abstractions.Model.Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);

                foreach (var item in expectedCategory)
                {
                    var result = await rep.GetAsync(item.Code);
                    Assert.NotNull(result);

                    Compare(item, result);
                }

                context.FlushDatabase();
            }
        }

        [Theory]
        [InlineData("MA")]
        [InlineData("MA ")]
        [InlineData(" ma ")]
        [InlineData("")]
        [InlineData(null)]
        internal async void GetAsyncByCode_InValid(string code)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);

                var result = await rep.GetAsync(code);
                Assert.Null(result);
                
                context.FlushDatabase();
            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidDefaults))]
        internal async void GetAsyncById_Valid(Abstractions.Model.Category[] expectedCategory)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);

                foreach (var item in expectedCategory)
                {
                    var result = await rep.GetAsync(item.Id.Value);
                    Assert.NotNull(result);

                    Compare(item, result);
                }

                context.FlushDatabase();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        internal async void GetAsyncById_InValid(int id)
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);

                var result = await rep.GetAsync(id);
                Assert.Null(result);

                context.FlushDatabase();
            }
        }

        [Theory]
        [ClassData(typeof(GenerateValidSave))]
        internal async void Save_Valid(Abstractions.Model.Category category, Abstractions.Model.Category expected)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);

                    var allCategories = await rep.GetAsync();

                    var result = await rep.SaveAsync(category);
                    Compare(result, expected);

                    var getByCode = await rep.GetAsync(expected.Code);
                    Compare(getByCode, expected);

                    var getById = await rep.GetAsync(expected.Id.Value);
                    Compare(getById, expected);
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
        [ClassData(typeof(GenerateValidDelete))]
        internal async void Delete_Valid(Abstractions.Model.Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);

                    var result = await rep.DeleteAsync(category);
                    Assert.True(result);

                    var getByCode = await rep.GetAsync(category.Code);
                    Assert.Null(getByCode);

                    var getById = await rep.GetAsync(category.Id.Value);
                    Assert.Null(getById);
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
        [ClassData(typeof(GenerateInValidUpdate))]
        internal async void Save_InValid(Abstractions.Model.Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.SaveAsync(category));
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
        [ClassData(typeof(GenerateInValidCreate))]
        internal async void Create_InValid(Abstractions.Model.Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.SaveAsync(category));
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
        [ClassData(typeof(GenerateInValidDelete))]
        internal async void Delete_InValid(Abstractions.Model.Category category)
        {
            using (var context = Storage.CreateContext())
            {
                try
                {
                    var rep = new CategoryRepository(context);

                    await Assert.ThrowsAnyAsync<Exception>(() => rep.DeleteAsync(category));
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

        [Fact]
        internal async void CheckIsViewWorking()
        {
            using (var context = Storage.CreateContext())
            {
                var rep = new CategoryRepository(context);

                var cat = await rep.GetAsync("lit");
                Assert.Equal(1, cat.TotalProjects);

                context.FlushDatabase();
            }
        }



        private void Compare(Abstractions.Model.Category category, Abstractions.Model.Category expected)
        {
            Assert.Equal(category.Code, expected.Code);
            Assert.Equal(category.DisplayName, expected.DisplayName);
            Assert.Equal(category.IsEverything, expected.IsEverything);
            Assert.Equal(category.Version, expected.Version);
        }
    }
}

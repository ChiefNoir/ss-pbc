using GeneralTests.Common;
using Infrastructure.DataModel;
using Infrastructure.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeneralTests.Functions.Infrastructure.Repository
{
    public class CategoryRepository_Tests
    {
        class GenerateValidGet : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new object[] 
                    {
                        new Category
                        {
                            Code = "red",
                            DisplayName = "Red name",
                            IsEverything = true,
                            Version = 0
                        },
                        new Category
                        {
                            Code = "system",
                            DisplayName = "System name",
                            IsEverything = true,
                            Version = 0
                        },
                        new Category
                        {
                            Code = "blue",
                            DisplayName = "blue name",
                            IsEverything = true,
                            Version = 0
                        }
                    },
                    new Abstractions.Model.Category
                    {
                        Code = "system",
                        DisplayName = "System name",
                        IsEverything = true,
                        Version = 0,
                    }
                };

            }
        }


        [Theory]
        [ClassData(typeof(GenerateValidGet))]
        internal async void GetAsync_Valid(Category[] initialItems, Abstractions.Model.Category expected)
        {
            using (var context = Utils.CreateContext(initialItems))
            {
                var rep = new CategoryRepository(context);
                var result = await rep.GetTechnicalAsync();

                Compare(result, expected);

                context.Database.EnsureDeleted();
            }


        }

        private void Compare(Abstractions.Model.Category category, Abstractions.Model.Category expected)
        {
            Assert.Equal(category.Code, expected.Code);
            Assert.Equal(category.DisplayName, expected.DisplayName);
            Assert.Equal(category.IsEverything, expected.IsEverything);
            Assert.Equal(category.TotalProjects, expected.TotalProjects);
            Assert.Equal(category.Version, expected.Version);
        }
    }
}

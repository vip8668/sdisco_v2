using TepayLink.Sdisco.Auditing;
using TepayLink.Sdisco.Test.Base;
using Shouldly;
using Xunit;

namespace TepayLink.Sdisco.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("TepayLink.Sdisco.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("TepayLink.Sdisco.Auditing.GenericEntityService`1[[TepayLink.Sdisco.Storage.BinaryObject, TepayLink.Sdisco.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("TepayLink.Sdisco.Auditing.XEntityService`1[TepayLink.Sdisco.Auditing.AService`5[[TepayLink.Sdisco.Storage.BinaryObject, TepayLink.Sdisco.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[TepayLink.Sdisco.Storage.TestObject, TepayLink.Sdisco.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}

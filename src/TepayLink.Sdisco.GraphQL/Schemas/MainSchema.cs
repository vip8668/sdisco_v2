using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using TepayLink.Sdisco.Queries.Container;

namespace TepayLink.Sdisco.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}
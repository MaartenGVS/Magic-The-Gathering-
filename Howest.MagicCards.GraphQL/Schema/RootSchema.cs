using GraphQL.Types;
using Howest.MagicCards.GraphQL.Query;

namespace Howest.MagicCards.GraphQL
{
    public class RootSchema: Schema 
    {
        public RootSchema(IServiceProvider provider): base(provider)
        {
            Query = provider.GetRequiredService<RootQuery>();
        }
    }
}

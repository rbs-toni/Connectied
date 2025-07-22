using Ardalis.Specification;
using Connectied.Domain.GuestLists;

namespace Connectied.Application.GuestLists.Specifications;

sealed class GuestListByNameSpecs : Specification<GuestList>
{
    public GuestListByNameSpecs(string name)
    {
        Query.Where(gl => gl.Name == name);
    }
}

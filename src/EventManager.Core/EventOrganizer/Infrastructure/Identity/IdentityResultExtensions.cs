using Microsoft.AspNetCore.Identity;
using SharedKernel.Models;
using System.Linq;

namespace EventOrganizer.Infrastructure.Identity
{
  public static class IdentityResultExtensions
  {
    public static Result ToApplicationResult(this IdentityResult result)
    {
      return result.Succeeded
          ? Result.Success()
          : Result.Failure(result.Errors.Select(e => e.Description));
    }
  }
}
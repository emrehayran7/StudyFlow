using StudyFlow.Core.Helper;

namespace StudyFlow.Tests
{
    public sealed class FakeCurrentUserService : ICurrentUserService
    {
        private readonly int _userId;

        public FakeCurrentUserService(int userId)
        {
            _userId = userId;
        }

        public int GetUserId()
        {
            return _userId;
        }
    }
}

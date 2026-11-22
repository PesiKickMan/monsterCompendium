using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Services;

namespace monsterCompendium.Tests.Fakes
{
    public class FakePhotoService : IPhotoService
    {
        public string? PhotoPathToReturn { get; set; }

        public int CaptureCallCount { get; private set; }

        public string? LastMonsterIndex { get; private set; }

        public Task<string?> CapturePhotoAsync(
        string monsterIndex)
        {
            CaptureCallCount++;

            LastMonsterIndex =
            monsterIndex;

            return Task.FromResult(
            PhotoPathToReturn);
        }
    }
}
